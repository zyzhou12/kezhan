using Aliyun.OSS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
            HttpPostedFile imgFile = context.Request.Files["file"];
            if (imgFile != null)
            {
                var endpoint = "oss-cn-hangzhou.aliyuncs.com";
                var accessKeyId = "LTAI0W5pqyqDXHhs"; //LTAIlLsb3W0Idk6a
                var accessKeySecret = "c2sUv3Lf3hNr1DSsQdb3KqYcMQiGlD "; //EGCWeEQlwLqLaIGZRYrfEmcpPInQCV 
                var bucketName = "aizhu-ducation";

                ossClient = new OssClient(endpoint, accessKeyId, accessKeySecret);
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
                context.Response.Write(key);//
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