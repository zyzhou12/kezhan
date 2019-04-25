using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.IO;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Net;
using System.Web;
using TiKuService.Model;
using System.Web.Script.Serialization;
using TiKu.Entity;
using TiKu.Dal;
using TiKuBll;
using TiKuBll.Model;
using TiKuService.Code;
using TiKu.Bll;

namespace TiKuService
{
    public partial class MediaService : ServiceBase
    {
        public MediaService()
        {
            InitializeComponent();
        }

        // 变量定义
        // private static ProcessStartInfo info = new ProcessStartInfo("cmd.exe");
        private static readonly string CurrentPath = "C:\\TiKuServiceLog" + "\\";

        //private static readonly string CurrentPath = System.Environment.CurrentDirectory;
        private Thread thread;
        private bool isNeedStop;

        protected override void OnStart(string[] args)
        {
            // 服务启动
            SaveLog("The TiKu Service is Starting On\t" + DateTime.Now.ToString());
            try
            {
                isNeedStop = false;
                thread = new Thread(new ThreadStart(RunLoop));
                thread.Start();

            }
            catch (Exception ex)
            {
                SaveLog("The TiKu Service is Error\t" + DateTime.Now.ToString() + "\t" + ex.Message);
            }

        }


        protected override void OnStop()
        {
            isNeedStop = true;
        }



        protected void RunLoop()
        {
            while (true)
            {


                try
                {

                    // xyk meet hz sy sxgg
                    string connstr, connbasestr, strSql;
                    connstr = "server=rm-bp1335dr8hlt581150o.sqlserver.rds.aliyuncs.com,3433;database=kezhan2;uid=aizhu;pwd=Aizhu@777;Max Pool Size = 512";
                    connbasestr = "server=rm-bp1335dr8hlt581150o.sqlserver.rds.aliyuncs.com,3433;database=kezhan2;uid=aizhu;pwd=Aizhu@777;Max Pool Size = 512";


                    SqlConnection conn;
                    conn = new SqlConnection(connstr);
                    SqlConnection connBase;
                    connBase = new SqlConnection(connbasestr);
                    try
                    {
                        conn.Open();
                        connBase.Open();
                    }
                    catch (Exception ex)
                    {
                        SaveLog("conn is Error\t" + DateTime.Now.ToString() + "\t" + ex.Message);
                        conn.Close();
                        conn.Dispose();
                        connBase.Close();
                        connBase.Dispose();
                    }



                    // sms procedure
                    strSql = "exec  [QueryCourseMedia]";
                    SqlDataAdapter daSMS = new SqlDataAdapter(strSql, conn);
                    DataSet dsSms = new DataSet();

                    daSMS.Fill(dsSms);


                    if (dsSms.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i <= dsSms.Tables[0].Rows.Count - 1; i++)
                        {
                            int iCourseId = Convert.ToInt32(dsSms.Tables[0].Rows[i]["fID"]);
                            string strUrl = dsSms.Tables[0].Rows[i]["fResourceUrl"].ToString();
                            // send mail

                            try
                            {

                                SaveLog("The QueryMediaListByURL is Begin" + DateTime.Now.ToString() + "\t" + iCourseId.ToString() + "\t" + strUrl);
                                QueryMediaListByURL(iCourseId, strUrl);

                                SaveLog("The QueryMediaListByURL is Send\t" + DateTime.Now.ToString() + "\t" + iCourseId.ToString() + "\t" + strUrl);

                            }
                            catch (Exception ex)
                            {
                                SaveLog("The  QueryMediaListByURL Service is Error\t" + DateTime.Now.ToString() + "\t" + ex.Message);
                            }


                        }
                    }
                    strSql = @"select b.* from tbooking b
                        left join tClassRoom r on r.fClassRoomCode=b.fTypeCode
                        where b.fType='ClassRoom' and b.fStatus='提交' and r.fPayType='在线支付' and b.fCreateDate<dateadd(mi,-30,getdate())";
                    daSMS = new SqlDataAdapter(strSql, conn);
                    dsSms = new DataSet();

                    daSMS.Fill(dsSms);


                    if (dsSms.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i <= dsSms.Tables[0].Rows.Count - 1; i++)
                        {
                            string bookingNo = dsSms.Tables[0].Rows[i]["fBookingNo"].ToString();
                            try
                            {

                                SaveLog("The UpdateBookingStatus is Begin" + DateTime.Now.ToString() + "\t" + bookingNo);
                                UpdateBookingStatus(bookingNo);


                                SaveLog("The UpdateBookingStatus is end\t" + DateTime.Now.ToString() + "\t" + bookingNo);

                            }
                            catch (Exception ex)
                            {
                                SaveLog("The  UpdateBookingStatus Service is Error\t" + DateTime.Now.ToString() + "\t" + ex.Message);
                            }


                        }
                    }

                    //获取群自动解散（老师退出后三小时）
                    strSql = @" select * from tgroup 
                                 where fgroupid in (
                                 select fClassRoomCode+Convert(varchar(10),fid) from tCourse where dateadd(minute,fClassDateLength+5,fClassDate)<getdate())
                                 and fIsValid=1";
                    daSMS = new SqlDataAdapter(strSql, conn);
                    dsSms = new DataSet();

                    daSMS.Fill(dsSms);


                    if (dsSms.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i <= dsSms.Tables[0].Rows.Count - 1; i++)
                        {
                            string groupID = dsSms.Tables[0].Rows[i]["fGroupID"].ToString();
                            try
                            {

                                SaveLog("The UpdateGroupStatus is Begin" + DateTime.Now.ToString() + "\t" + groupID);
                                UpdateGroupStatus(groupID);


                                SaveLog("The UpdateGroupStatus is end\t" + DateTime.Now.ToString() + "\t" + groupID);

                            }
                            catch (Exception ex)
                            {
                                SaveLog("The  UpdateGroupStatus Service is Error\t" + DateTime.Now.ToString() + "\t" + ex.Message);
                            }


                        }
                    }




                    //获取群成员状态
                    strSql = @"select u.fGroupID,count(*) from tgroupuserinfo u
                        left join tGroup g on g.fGroupID=u.fGroupID
                        where dateadd(minute,5,fDestoryDate)>getdate() or fIsValid=1 group by u.fGroupID";
                    daSMS = new SqlDataAdapter(strSql, conn);
                    dsSms = new DataSet();

                    daSMS.Fill(dsSms);

                    if (dsSms.Tables.Count > 0 && dsSms.Tables[0] != null)
                    {
                        if (dsSms.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i <= dsSms.Tables[0].Rows.Count - 1; i++)
                            {
                                string groupID = dsSms.Tables[0].Rows[i]["fGroupID"].ToString();
                                try
                                {
                                    SaveLog("The UpdateGroupUserStatus is Begin" + DateTime.Now.ToString() + "\t" + groupID);
                                    UpdateGroupUserStatus(groupID);

                                    SaveLog("The UpdateGroupUserStatus is end\t" + DateTime.Now.ToString() + "\t" + groupID);

                                }
                                catch (Exception ex)
                                {
                                    SaveLog("The  UpdateGroupUserStatus Service is Error\t" + DateTime.Now.ToString() + "\t" + ex.Message);
                                }
                            }
                        }
                    }
                    try
                    {
                        //检查流量是否过期
                        UserBll.CheckFlowEffect();
                    }
                    catch (Exception ex)
                    {
                        SaveLog("The  CheckFlowEffect Service is Error\t" + DateTime.Now.ToString() + "\t" + ex.Message);
                    }

                    conn.Close();
                    conn.Dispose();
                }
                catch (Exception ex)
                {
                    SaveLog("The  Service is Error\t" + ex.Message);
                }

                if (this.isNeedStop) break;
                Thread.Sleep(1 * 1000);
            }
        }


        //
        /// <summary>
        /// 获取md5码
        /// </summary>
        /// <param name="source">待转换字符串</param>
        /// <returns>md5加密后的字符串</returns>
        public string getMD5(string source)
        {
            string result = "";
            try
            {
                MD5 getmd5 = new MD5CryptoServiceProvider();
                byte[] targetStr = getmd5.ComputeHash(UnicodeEncoding.UTF8.GetBytes(source));
                result = BitConverter.ToString(targetStr).Replace("-", "");
                return result;
            }
            catch (Exception)
            {
                return "0";
            }

        }


        // 日志
        public void SaveLog(string msg)
        {
            try
            {
                string strPath = CurrentPath + "\\TiKuService.log";
                if (!Directory.Exists(strPath)) Directory.CreateDirectory(strPath);
                string strFileName = strPath + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
                StreamWriter sw = File.AppendText(strFileName);
                sw.WriteLine(msg);
                sw.Close();
            }
            catch
            {
            }
        }

        /// <summary>  
        /// Post数据到网站  
        /// </summary>  
        /// <param name="posturl">网址</param>  
        /// <param name="postData">参数</param>  
        /// <returns></returns>  
        public string GetPage(string posturl, string postData)
        {
            Stream outstream = null;
            Stream instream = null;
            StreamReader sr = null;
            HttpWebResponse response = null;
            HttpWebRequest request = null;
            Encoding encoding = System.Text.Encoding.GetEncoding("UTF-8");
            byte[] data = encoding.GetBytes(postData);
            // 准备请求...  
            try
            {
                // 设置参数  
                request = WebRequest.Create(posturl) as HttpWebRequest;
                CookieContainer cookieContainer = new CookieContainer();
                request.CookieContainer = cookieContainer;
                request.AllowAutoRedirect = true;
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;
                outstream = request.GetRequestStream();
                outstream.Write(data, 0, data.Length);
                outstream.Close();
                //发送请求并获取相应回应数据  
                response = request.GetResponse() as HttpWebResponse;
                //直到request.GetResponse()程序才开始向目标网页发送Post请求  
                instream = response.GetResponseStream();
                sr = new StreamReader(instream, encoding);
                //返回结果网页（html）代码  
                string content = sr.ReadToEnd();
                string err = string.Empty;
                return content;
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                return string.Empty;
            }
        }

        public string GetWebData(string DataUrl)
        {
            try
            {


                WebRequest GetKey;
                GetKey = WebRequest.Create(DataUrl);
                Stream objStream;
                objStream = GetKey.GetResponse().GetResponseStream();

                StreamReader objReader = new StreamReader(objStream);

                string sLine = "";

                sLine = objReader.ReadLine();

                return sLine;
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                return string.Empty;
            }


        }


        public void QueryMediaListByURL(int iCourseId, string strUrl)
        {
            Dictionary<string, string> parameterMap = new Dictionary<string, string>();
            // 请求公共参数
            parameterMap.Add("Version", "2014-06-18");
            parameterMap.Add("AccessKeyId", "LTAI0W5pqyqDXHhs"); //此处请替换成您自己的AccessKeyId
            // parameterMap.Add("Timestamp", "2015-05-14T09:03:45Z");//此处将时间戳固定只是测试需要，这样此示例中生成的签名值就不会变，方便您对比验证，可变时间戳的生成需要用下边这句替换
            parameterMap.Add("Timestamp", DateTime.Now.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ"));
            parameterMap.Add("SignatureMethod", "HMAC-SHA1");
            parameterMap.Add("SignatureVersion", "1.0");
            // parameterMap.Add("SignatureNonce", "4902260a-516a-4b6a-a455-45b653cf6150"); //此处将唯一随机数固定只是测试需要，这样此示例中生成的签名值就不会变，方便您对比验证，可变唯一随机数的生成需要用下边这句替换
            parameterMap.Add("SignatureNonce", System.Guid.NewGuid().ToString());
            parameterMap.Add("Format", "JSON"); //另外支持JSON格式



            // 加入方法特有参数
            parameterMap.Add("Action", "QueryMediaListByURL");
            parameterMap.Add("FileURLs", "http://aizhu-ducation.oss-cn-hangzhou.aliyuncs.com/" + AliSign.encodeByRFC3986(strUrl));
            parameterMap.Add("IncludePlayList", "true");
            parameterMap.Add("IncludeSnapshotList", "true");
            parameterMap.Add("IncludeMediaInfo", "true");



            string canonicalizedQueryString = AliSign.buildCanonicalizedQueryString(parameterMap);
            string stringToSign = AliSign.buildStringToSign(canonicalizedQueryString);
            string signature = AliSign.buildSignature("c2sUv3Lf3hNr1DSsQdb3KqYcMQiGlD&", stringToSign);
            string url = AliSign.buildRequestURL(signature, parameterMap);

            string ufResult = GetWebData(url);

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            MediaResponseModel response = serializer.Deserialize<MediaResponseModel>(ufResult);
            if (response.MediaList != null)
            {
                if (response.MediaList.Media != null && response.MediaList.Media.Count > 0)
                {
                    string CoverURL = "";
                    // "MediaList":{"Media":[{"CoverURL":"http://kezhan.oss-cn-hangzhou.aliyuncs.com/Act-Snapshot/708e63a6c3084002b98f6a65a52dcdbf/1000.jpg","Format":"mov,mp4,m4a,3gp,3g2,mj2","PublishState":"UnPublish","File":{"State":"Normal","URL":"http://aizhu-ducation.oss-cn-hangzhou.aliyuncs.com/mp4MultibitrateIn40/v020040b0000bf42ffsd1dr4mvqfm01g.mp4"},"SnapshotList":{"Snapshot":[{"MediaWorkflowName":"新建工作流1544070748909","Count":1,"Type":"Single","File":{"State":"Normal","URL":"http://kezhan.oss-cn-hangzhou.aliyuncs.com/Act-Snapshot/708e63a6c3084002b98f6a65a52dcdbf/1000.jpg"},"MediaWorkflowId":"f70b62689cb9428ea22f0b35ec83483f","ActivityName":"Act-Snapshot"}]},"Height":"480","PlayList":{"Play":[{"MediaWorkflowName":"新建工作流1544070748909","Format":"mp4","File":{"State":"Normal","URL":"http://kezhan.oss-cn-hangzhou.aliyuncs.com/Act-ss-mp4-ld/708e63a6c3084002b98f6a65a52dcdbf/v020040b0000bf42ffsd1dr4mvqfm01g.mp4"},"Duration":"447","Encryption":"0","Height":"362","Width":"640","MediaWorkflowId":"f70b62689cb9428ea22f0b35ec83483f","Fps":"10","ActivityName":"Act-ss-mp4-ld","Bitrate":"147","Size":"8219377"},{"MediaWorkflowName":"新建工作流1544070748909","Format":"mp4","File":{"State":"Normal","URL":"http://kezhan.oss-cn-hangzhou.aliyuncs.com/Act-ss-mp4-sd/708e63a6c3084002b98f6a65a52dcdbf/v020040b0000bf42ffsd1dr4mvqfm01g.mp4"},"Duration":"447","Encryption":"0","Height":"478","Width":"848","MediaWorkflowId":"f70b62689cb9428ea22f0b35ec83483f","Fps":"10","ActivityName":"Act-ss-mp4-sd","Bitrate":"181","Size":"10130277"},{"MediaWorkflowName":"新建工作流1544070748909","Format":"mp4","File":{"State":"Normal","URL":"http://kezhan.oss-cn-hangzhou.aliyuncs.com/Act-ss-mp4-hd/708e63a6c3084002b98f6a65a52dcdbf/v020040b0000bf42ffsd1dr4mvqfm01g.mp4"},"Duration":"447","Encryption":"0","Height":"722","Width":"1280","MediaWorkflowId":"f70b62689cb9428ea22f0b35ec83483f","Fps":"10","ActivityName":"Act-ss-mp4-hd","Bitrate":"262","Size":"14666058"}]},"MediaId":"c10f1640d65a4e9b9adad7f9f60f7163","Title":"v020040b0000bf42ffsd1dr4mvqfm01g.mp4","CreationTime":"2018-12-10T05:26:36Z","RunIdList":{"RunId":["708e63a6c3084002b98f6a65a52dcdbf"]},"CateId":0,"MediaInfo":{"Format":{"FormatName":"mov,mp4,m4a,3gp,3g2,mj2","Duration":"446.960000","FormatLongName":"QuickTime / MOV","NumStreams":"2","StartTime":"-0.114694","Bitrate":"82.918","NumPrograms":"0","Size":"4632651"},"Streams":{"SubtitleStreamList":{"SubtitleStream":[]},"AudioStreamList":{"AudioStream":[{"Lang":"und","SampleFmt":"fltp","CodecName":"aac","CodecTimeBase":"1/44100","Timebase":"1/44100","CodecTag":"0x6134706d","Channels":"2","ChannelLayout":"stereo","Index":"1","CodecTagString":"mp4a","Samplerate":"44100","Duration":"446.959705","CodecLongName":"AAC (Advanced Audio Coding)","StartTime":"-0.114694","Bitrate":"64.003"}]},"VideoStreamList":{"VideoStream":[{"Lang":"und","PixFmt":"yuv420p","NetworkCost":{},"Dar":"0:1","Profile":"High","Height":"480","Sar":"0:1","CodecName":"h264","Timebase":"1/10240","CodecTimeBase":"1/20","CodecTag":"0x31637661","HasBFrames":"2","Index":"0","CodecTagString":"avc1","Duration":"446.900000","AvgFPS":"10.0","CodecLongName":"H.264 / AVC / MPEG-4 AVC / MPEG-4 part 10","Level":"22","StartTime":"0.000000","Width":"850","Fps":"10.0","Bitrate":"16.315"}]}}},"CensorState":"Initiated","Duration":"446.960000","Width":"850","Fps":"10.0","Bitrate":"82.918","Size":"4632651"}]},"RequestId":"B3B4722E-1B87-4519-A432-CF5B6E3EC699","NonExistFileURLs":{"FileURL":[]}}
                    List<tMediaEntity> list = new List<tMediaEntity>();
                    foreach (var media in response.MediaList.Media)
                    {
                        CoverURL = media.CoverURL;
                        foreach (var play in media.PlayList.Play)
                        {
                            tMediaEntity entity = new tMediaEntity();
                            entity.fActivityName = play.ActivityName;
                            entity.fCourseId = iCourseId;
                            entity.fHeight = play.Height;
                            entity.fMediaID = media.MediaId;
                            entity.fSize = play.Size;
                            entity.fUrl = play.File.URL;
                            entity.fWidth = play.Width;
                            list.Add(entity);
                        }
                    }

                    if (list.Count > 0)
                    {
                        int i = tMediaDal.Modify(list, "insert", null, null);
                        tCourseEntity course = new tCourseEntity();
                        course.fID = iCourseId;
                        course.fStatus = "已转码";
                        course.fFileCoverUrl = CoverURL;
                        List<tCourseEntity> cList = new List<tCourseEntity>();
                        cList.Add(course);
                        tCourseDal.Modify(cList, "update", "fID,fStatus,fFileCoverUrl", null);
                    }
                }
            }
        }


        /// <summary>
        /// 自动解散课堂
        /// </summary>
        /// <param name="strGroupId"></param>
        public void UpdateGroupStatus(string strGroupId)
        {
            Random rand = new Random();
            StringBuilder sb = new StringBuilder();
            for (int i = 1; i <= 32; i++)
            {
                int randNum = rand.Next(9) + 1;
                String num = randNum + "";
                sb = sb.Append(num);
            }
            String random = sb.ToString();

            string strUrl = string.Format("https://console.tim.qq.com/v4/group_open_http_svc/destroy_group?usersig={0}&identifier={1}&sdkappid={2}&random={3}&contenttype=json", "eJxlj81Og0AYRfc8BWFtdH6YlJp0gbYKpiQaalPZkJEZ8MOUgZmhrW18dyPWSOLdnpN7c0*O67reaple8qJQfWNz*9FKz712PeRd-MG2BZFzm1Mt-kF5aEHLnJdW6gFixhhBaOyAkI2FEs4GF1towFjNrdIjzYj3fNj66fERwpOABdOxAtUAk8XzbRy9vFVwtA*PEcN9TFc89LskocfX4t4v*TwV6-0d3WB-sxMhLELDg7kiKTGRikxfP10texTd1EqldWYO-jreV12msrqboNlo0sJW-h6jmAVkOr62k9qAagaBIMwwoeg7nvPpfAFRuGD7", "administrator", 1400178589, random);
            string strBody ="{\"GroupId\": \"" + strGroupId + "\"}";

            byte[] postData = Encoding.UTF8.GetBytes(strBody);
            WebClient MyWebClient = new WebClient();

            MyWebClient.Credentials = CredentialCache.DefaultCredentials;
            Byte[] pageData = MyWebClient.UploadData(strUrl, "post", postData);

            String strJson = Encoding.UTF8.GetString(pageData) ?? "";

            JavaScriptSerializer jss = new JavaScriptSerializer();

            GroupDestoryResponseModel response = jss.Deserialize<GroupDestoryResponseModel>(strJson);
            if (response.ActionStatus == "OK")
            {
                GroupUserBll.UpdateGroup(strGroupId, "closegroup");
            }

        }

        public void UpdateGroupUserStatus(string strGroupId)
        {
            Random rand = new Random();
            StringBuilder sb = new StringBuilder();
            for (int i = 1; i <= 32; i++)
            {
                int randNum = rand.Next(9) + 1;
                String num = randNum + "";
                sb = sb.Append(num);
            }
            String random = sb.ToString();

            string strUrl = string.Format("https://console.tim.qq.com/v4/openim/querystate?usersig={0}&identifier={1}&sdkappid={2}&random={3}&contenttype=json", "eJxlj81Og0AYRfc8BWFtdH6YlJp0gbYKpiQaalPZkJEZ8MOUgZmhrW18dyPWSOLdnpN7c0*O67reaple8qJQfWNz*9FKz712PeRd-MG2BZFzm1Mt-kF5aEHLnJdW6gFixhhBaOyAkI2FEs4GF1towFjNrdIjzYj3fNj66fERwpOABdOxAtUAk8XzbRy9vFVwtA*PEcN9TFc89LskocfX4t4v*TwV6-0d3WB-sxMhLELDg7kiKTGRikxfP10texTd1EqldWYO-jreV12msrqboNlo0sJW-h6jmAVkOr62k9qAagaBIMwwoeg7nvPpfAFRuGD7", "administrator", 1400178589, random);
            StringBuilder strUserIds = new StringBuilder();
            strUserIds.Append("{\"To_Account\": [");
            GroupUserInfoListModel model = GroupUserBll.GetGroupUserInfiList(strGroupId);
            foreach (GroupUserInfoModel info in model.infoList)
            {
                strUserIds.Append("\"" + info.fUserId + "\",");
            }
            strUserIds.Append("\"\"]}");

            byte[] postData = Encoding.UTF8.GetBytes(strUserIds.ToString());
            WebClient MyWebClient = new WebClient();

            MyWebClient.Credentials = CredentialCache.DefaultCredentials;
            Byte[] pageData = MyWebClient.UploadData(strUrl, "post", postData);

            String strJson = Encoding.UTF8.GetString(pageData) ?? "";

            JavaScriptSerializer jss = new JavaScriptSerializer();

            GroupUserStatusModel groupUserStatusModel = jss.Deserialize<GroupUserStatusModel>(strJson);
            if (groupUserStatusModel.ActionStatus == "OK")
            {
                foreach (QueryResultModel result in groupUserStatusModel.QueryResult)
                {
                    if (!string.IsNullOrEmpty(result.To_Account))
                    {
                        if (result.State == "Offline")
                        {
                            GroupUserBll.UpdateGroupUserInfo(result.To_Account, strGroupId, "offline");
                        }
                    }
                }
            }
        }

        public void UpdateBookingStatus(string strBookingNo)
        {
            BookingBll.BookingUpdateStatus(strBookingNo, "已取消", "支付时间到自动取消", "System");
        }
    }
}
