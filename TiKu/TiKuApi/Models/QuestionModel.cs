using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TiKu.Api.Models
{
  public class QuestionModel
  {
    public string errorCode { get; set; }
    public List<Question> data { get; set; }
  }

  public class  Question
  {
    public string title { get; set; }
    public string option_a { get; set; }
    public string option_b { get; set; }
    public string option_c { get; set; }
    public string option_d { get; set; }
    public string qType { get; set; }
    public decimal diff { get; set; }
    public int year { get; set; }
    public string source { get; set; }
    public int subjectId { get; set; }
    public string paperTypeId { get; set; }
    public string qid { get; set; }
    public int isSub { get; set; }
    public int isNormal { get; set; }
  }
}