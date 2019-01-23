using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TiKu.Api.Models
{
  public class AnswerModel
  {
    public string errorCode { get; set; }
    public List<Answer> data { get; set; }
  }

  public class Answer
  {
    public string answer1{get;set;}
    public string answer2{get;set;}
    public string parse { get; set; }
  }
}