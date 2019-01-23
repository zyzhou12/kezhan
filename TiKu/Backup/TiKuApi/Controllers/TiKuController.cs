using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.IO;
using System.Text;
using TiKu.Api.Code;
using TiKu.Api.Models;
using System.Web.Script.Serialization;
using TiKu.Dal;
using TiKu.Entity;

namespace TiKu.Api.Controllers
{
    public class TiKuController : Controller
    {

      //
      // GET: /shiti/

      /// <summary>
      /// 学段、年级、科目、版本基础信息
      /// </summary>
      /// <returns></returns>
      public string subjectEditionApi()
      {
        string strUrl = "http://www.xuekubao.com/index.php?s=Index&m=Api&a=subjectEditionApi";

        var parameters1 = new Dictionary<string, string>();

        parameters1.Add("accessKey", "1cb09646f06c607a05b9a539d793c1d0");

        string strJson = sendPost(strUrl, parameters1, "post");

        UTF8String str = new UTF8String(strJson);
        strJson = str.ToString();
        
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        SubjectEditionModel model = serializer.Deserialize<SubjectEditionModel>(strJson);

        List<tPharseEntity> pharseList=new List<tPharseEntity>();
        foreach(PharseModel pharse in model.data)
        {
          tPharseEntity p=new tPharseEntity();
          p.fPharseID=pharse.id;
          p.fPharseName=pharse.name;
          pharseList.Add(p);

          List<tGradeEntity> gradeList = new List<tGradeEntity>();
          foreach (GradeModel grade in pharse.child)
          {
            tGradeEntity g = new tGradeEntity();
            g.fGradeID = grade.id;
            g.fGradeName = grade.name;
            g.fPharseID = grade.pid;
            gradeList.Add(g);

            List<tSubjectEntity> subjectList = new List<tSubjectEntity>();
            foreach (SubjectModel subject in grade.child)
            {
              tSubjectEntity s = new tSubjectEntity();
              s.fSubjectID = subject.id;
              s.fSubjectname = subject.name;
              s.fGradeID = subject.pid;
              s.fPinYin = "";
              subjectList.Add(s);

              List<tEditionEntity> editionList = new List<tEditionEntity>();
              foreach (EditionModel edition in subject.child)
              {
                tEditionEntity e = new tEditionEntity();
                e.fID = edition.id;
                e.fName = edition.name;
                e.fSubjectID = edition.pid;
                editionList.Add(e);

               // chapterApi(p.fPharseID, s.fSubjectID, e.fID, g.fGradeID);
              }
              //tEditionDal.Modify(editionList, "insert", null, null);
            }
           // tSubjectDal.Modify(subjectList, "insert", null, null);
          }
         // tGradeDal.Modify(gradeList, "insert", null, null);
        }
       // tPharseDal.Modify(pharseList, "insert", null, null);


      return strJson;
      }

      /// <summary>
      /// 题型、难易度、试卷类型信息
      /// </summary>
      /// <returns></returns>
      public string getOtherBasic()
      {
        string strUrl = "http://www.xuekubao.com/index.php?s=Index&m=Api&a=getOtherBasic";

        var parameters1 = new Dictionary<string, string>();

        parameters1.Add("accessKey", "1cb09646f06c607a05b9a539d793c1d0");

        string strJson = sendPost(strUrl, parameters1, "post");

        UTF8String str = new UTF8String(strJson);
        strJson = str.ToString();

        JavaScriptSerializer serializer = new JavaScriptSerializer();
        OtherBasicModel model = serializer.Deserialize<OtherBasicModel>(strJson);

        List<tQTypeEntity> qTypeList = new List<tQTypeEntity>();
        foreach (QTypeModel qType in model.qTypes)
        {
          tQTypeEntity q = new tQTypeEntity();
          q.fID = qType.id;
          q.fqType = qType.typeName;
          qTypeList.Add(q);
        }
        tQTypeDal.Modify(qTypeList, "insert", null, null);


        return strJson;
      }

      /// <summary>
      /// 获取章节数据API
      /// </summary>
      /// <returns></returns>
      public string chapterApi()
      {
        string strUrl = "http://www.xuekubao.com/index.php?s=Index&m=Api&a=chapterApi";

        var parameters1 = new Dictionary<string, string>();

        parameters1.Add("accessKey", "1cb09646f06c607a05b9a539d793c1d0");
        parameters1.Add("pharseId", "1");
        parameters1.Add("subjectId", "2");
        parameters1.Add("editionId", "1");
        parameters1.Add("gradeId", "131");

        string strJson = sendPost(strUrl, parameters1, "post");

        UTF8String str = new UTF8String(strJson);
        strJson = str.ToString();

        JavaScriptSerializer serializer = new JavaScriptSerializer();
        ChapterModel model = serializer.Deserialize<ChapterModel>(strJson);
        List<tChapterEntity> chapterList = new List<tChapterEntity>();
        foreach (ChapterData chapter in model.data)
        {
          tChapterEntity c = new tChapterEntity();
          c.fChapter = chapter.name;
          c.fChapterId = chapter.id;
          c.fEditionID = chapter.editionId;
          c.fGradeID = chapter.gradeId;
          c.fID = chapter.id;
          
          chapterList.Add(c);
        }
        tChapterDal.Modify(chapterList, "insert", null, null);

        return strJson;
      }

      /// <summary>
      /// 根据知识点取试题API
      /// </summary>
      /// <returns></returns>
      public string getQuestions()
      {
        string strUrl = "http://www.xuekubao.com/index.php?s=Index&m=Api&a=getQuestions";


        var parameters1 = new Dictionary<string, string>();

        parameters1.Add("accessKey", "1cb09646f06c607a05b9a539d793c1d0");
        parameters1.Add("knowledgeId", "d89e9d70424b9984336bb9aec61c4fb4");
        parameters1.Add("page", "1");

        WebClient MyWebClient = new WebClient();
        byte[] postData = Encoding.UTF8.GetBytes(BuildQuery(parameters1, "utf8"));


        MyWebClient.Credentials = CredentialCache.DefaultCredentials;
        MyWebClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded;charset=utf-8");
        byte[] pageData = MyWebClient.UploadData(strUrl, "post", postData);
        String strJson = Encoding.UTF8.GetString(pageData) ?? "";

       // UTF8String str = new UTF8String(strJson);
       // strJson = str.ToString();

        
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        QuestionModel model = serializer.Deserialize<QuestionModel>(strJson);
        List<tQuestionEntity> questionList = new List<tQuestionEntity>();
        foreach (Question question in model.data)
        {
          tQuestionEntity q = new tQuestionEntity();
          q.fTitle = new UTF8String(question.title).ToString();
          q.fDiff = question.diff;
          q.fIsNormal = question.isNormal;
          q.fIsSub = question.isSub;
          q.fOption_a = question.option_a;
          q.fOption_b = question.option_b;
          q.fOption_c = question.option_c;
          q.fOption_d = question.option_d;
          q.fPaperTpye = question.paperTypeId;
          q.fID= question.qid;
          q.fQtype = question.qType;
          q.fSource = question.source;
          q.fSubjectID = question.subjectId;
          q.fYear = question.year;
          //q.fKnowledges=questionk
          
          questionList.Add(q);
        }
        tQuestionDal.Modify(questionList, "insert", null, null);
        

        return strJson;
      }

      /// <summary>
      /// 根据试题ID获取答案解析API
      /// </summary>
      /// <returns></returns>
      public string getAnswer(string strQid)
      {
        string strUrl = "http://www.xuekubao.com/index.php?s=Index&m=Api&a=getAnswer";


        var parameters1 = new Dictionary<string, string>();

        parameters1.Add("accessKey", "1cb09646f06c607a05b9a539d793c1d0");
        parameters1.Add("qid", strQid);

        WebClient MyWebClient = new WebClient();
        byte[] postData = Encoding.UTF8.GetBytes(BuildQuery(parameters1, "utf8"));


        MyWebClient.Credentials = CredentialCache.DefaultCredentials;
        MyWebClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded;charset=utf-8");
        byte[] pageData = MyWebClient.UploadData(strUrl, "post", postData);
        String strJson = Encoding.UTF8.GetString(pageData) ?? "";

       // UTF8String str = new UTF8String(strJson);
      //  strJson = str.ToString();


        JavaScriptSerializer serializer = new JavaScriptSerializer();
        AnswerModel model = serializer.Deserialize<AnswerModel>(strJson);
        List<tQuestionEntity> questionList = new List<tQuestionEntity>();
        foreach (Answer answer in model.data)
        {
          tQuestionEntity q = new tQuestionEntity();
          q.fAnswer1 = new UTF8String(answer.answer1).ToString();
          q.fAnswer2 = new UTF8String(answer.answer2).ToString();
          q.fParse = new UTF8String(answer.parse).ToString();
          q.fID = strQid;

          questionList.Add(q);
        }
        tQuestionDal.Modify(questionList, "Update", "fID,fAnswer1,fAnswer2,fParse", null);
        

        return strJson;
      }



      /// <summary>
      /// Http (GET/POST)
      /// </summary>
      /// <param name="url">请求URL</param>
      /// <param name="parameters">请求参数</param>
      /// <param name="method">请求方法</param>
      /// <returns>响应内容</returns>
      static string sendPost(string url, IDictionary<string, string> parameters, string method)
      {
        if (method.ToLower() == "post")
        {
          HttpWebRequest req = null;
          HttpWebResponse rsp = null;
          System.IO.Stream reqStream = null;
          try
          {
            req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = method;
            req.KeepAlive = false;
            req.ProtocolVersion = HttpVersion.Version10;
            req.Timeout = 5000;
            req.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
            byte[] postData = Encoding.UTF8.GetBytes(BuildQuery(parameters, "utf8"));

            reqStream = req.GetRequestStream();
            reqStream.Write(postData, 0, postData.Length);
            rsp = (HttpWebResponse)req.GetResponse();
            Encoding encoding = Encoding.GetEncoding(rsp.CharacterSet);
            return GetResponseAsString(rsp, encoding);
          }
          catch (Exception ex)
          {
            return ex.Message;
          }
          finally
          {
            if (reqStream != null) reqStream.Close();
            if (rsp != null) rsp.Close();
          }
        }
        else
        {
          //创建请求
          HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url + "?" + BuildQuery(parameters, "utf8"));

          //GET请求
          request.Method = "GET";
          request.ReadWriteTimeout = 5000;
          request.ContentType = "text/html;charset=UTF-8";
          HttpWebResponse response = (HttpWebResponse)request.GetResponse();
          Stream myResponseStream = response.GetResponseStream();
          StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));

          //返回内容
          string retString = myStreamReader.ReadToEnd();
          return retString;
        }
      }

      /// <summary>
      /// 组装普通文本请求参数。
      /// </summary>
      /// <param name="parameters">Key-Value形式请求参数字典</param>
      /// <returns>URL编码后的请求数据</returns>
      static string BuildQuery(IDictionary<string, string> parameters, string encode)
      {
        StringBuilder postData = new StringBuilder();
        bool hasParam = false;
        IEnumerator<KeyValuePair<string, string>> dem = parameters.GetEnumerator();
        while (dem.MoveNext())
        {
          string name = dem.Current.Key;
          string value = dem.Current.Value;
          // 忽略参数名或参数值为空的参数
          if (!string.IsNullOrEmpty(name))//&& !string.IsNullOrEmpty(value)
          {
            if (hasParam)
            {
              postData.Append("&");
            }
            postData.Append(name);
            postData.Append("=");
            if (encode == "gb2312")
            {
              postData.Append(HttpUtility.UrlEncode(value, Encoding.GetEncoding("gb2312")));
            }
            else if (encode == "utf8")
            {
              postData.Append(HttpUtility.UrlEncode(value, Encoding.UTF8));
            }
            else
            {
              postData.Append(value);
            }
            hasParam = true;
          }
        }
        return postData.ToString();
      }

      /// <summary>
      /// 把响应流转换为文本。
      /// </summary>
      /// <param name="rsp">响应流对象</param>
      /// <param name="encoding">编码方式</param>
      /// <returns>响应文本</returns>
      static string GetResponseAsString(HttpWebResponse rsp, Encoding encoding)
      {
        System.IO.Stream stream = null;
        StreamReader reader = null;
        try
        {
          // 以字符流的方式读取HTTP响应
          stream = rsp.GetResponseStream();
          reader = new StreamReader(stream, encoding);
          return reader.ReadToEnd();
        }
        finally
        {
          // 释放资源
          if (reader != null) reader.Close();
          if (stream != null) stream.Close();
          if (rsp != null) rsp.Close();
        }
      }

    }
}
