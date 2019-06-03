using Aliyun.OSS;
using KeZhan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TiKuBll;
using TiKuBll.Model;

namespace KeZhan.Handel
{
    /// <summary>
    /// OssUpload 的摘要说明
    /// </summary>
    public class OssUpload : IHttpHandler
    {

        OssClient ossClient;
        public void ProcessRequest(HttpContext context)
        {
            
            context.Response.ContentType = "text/plain";
            context.Response.Expires = -1;
            var endpoint = "oss-cn-hangzhou.aliyuncs.com";
            var accessKeyId = "LTAI0W5pqyqDXHhs"; //LTAIlLsb3W0Idk6a
            var accessKeySecret = "c2sUv3Lf3hNr1DSsQdb3KqYcMQiGlD "; //EGCWeEQlwLqLaIGZRYrfEmcpPInQCV 
            var bucketName = "aizhu-ducation";

            ossClient = new OssClient(endpoint, accessKeyId, accessKeySecret);
            HttpFileCollection files = context.Request.Files;
            if (files.Count > 0)
            {
                for (int i = 0; i < files.Count; i++)
                {
                    HttpPostedFile imgFile = files[i];

                    if (imgFile != null)
                    {

                        // ObjectMetadata meta = new ObjectMetadata();
                        // meta.ContentType = "image/jpeg";
                        //文件名称(源文件名称，重复会覆盖)
                        string key = context.Request["UserName"] + "/" + context.Request["FileType"] + "/" + DateTime.Now.ToString("yyyyMMddHHmm") + imgFile.FileName;
                        if (context.Request["FileType"] == "ClassRoom")//课堂视频
                        {
                            key = "ClassRoomMedia" + "/" + context.Request["UserName"] + "/" + DateTime.Now.ToString("yyyyMMddHHmm") + imgFile.FileName;
                        }
                        else if (context.Request["FileType"] == "CourseDocument")//课堂资料
                        {
                            key = "CourseDocument" + "/" + context.Request["UserName"] + "/" + DateTime.Now.ToString("yyyyMMddHHmm") + imgFile.FileName;
                        }
                        PutObjectResult result = ossClient.PutObject(bucketName, key, imgFile.InputStream);



                        // 获取文件元信息。
                        var oldMeta = ossClient.GetObjectMetadata(bucketName, key);
                        ResourseModel model = new ResourseModel();
                        model.fCreateDate = DateTime.Now;
                        model.fCreateOpr = context.Request["UserName"];
                        model.fFileType = oldMeta.ContentType;
                        model.fIsDownLoad = false;
                        model.fIsTrySee = false;
                        model.fPayIsDown = false;
                        model.fResourceCode = Guid.NewGuid().ToString();
                        model.fResourceTitle = imgFile.FileName;
                        model.fSize = Convert.ToInt32(oldMeta.ContentLength);
                        if (oldMeta.ContentType.Split('/')[0] == "image")
                        {
                            model.fCoverImg = string.Format("http://{0}.oss.aliyuncs.com/{1}?x-oss-process=style/fang", bucketName, key);
                        }
                        else if (oldMeta.ContentType.Split('/')[0] == "application")
                        {
                            model.fCoverImg = "https://aizhu-ducation.oss-cn-hangzhou.aliyuncs.com/WebImage/%E5%BE%AE%E4%BF%A1%E5%9B%BE%E7%89%87_20181221120133.png?x-oss-process=style/fang";
                        }

                        model.fStatus = "已上传";
                        model.fType = context.Request["FileType"];
                        model.fUrl = key;
                        model.fUserName = context.Request["UserName"];
                        ResourceBll.InsertResource(model);



                        AccessControlList accs = ossClient.GetBucketAcl(bucketName);
                        //string imgurl = string.Empty;
                        //if (!accs.Grants.Any())//判断是否有读取权限
                        //{
                        //    imgurl = ossClient.GeneratePresignedUri(bucketName, key, DateTime.Now.AddMinutes(5)).AbsoluteUri; //生成一个签名的Uri 有效期5分钟
                        //}
                        //else
                        //{

                        //    imgurl = string.Format("http://{0}.oss.aliyuncs.com/{1}", bucketName, key);
                        //} 
                        if (context.Request["FileType"] == "ClassRoom")//课堂视频
                        {
                            context.Response.Write(model.fResourceCode);
                        }
                        else
                        {
                            context.Response.Write(key);
                        }
                    }
                }
            }
            else
            {
                context.Response.Write("上传失败");
            }
        }

        // 获取上传进度。
        private static void streamProgressCallback(object sender, StreamTransferProgressArgs args)
        {
            System.Console.WriteLine("ProgressCallback - Progress: {0}%, TotalBytes:{1}, TransferredBytes:{2} ",
                args.TransferredBytes * 100 / args.TotalBytes, args.TotalBytes, args.TransferredBytes);
        }


        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}