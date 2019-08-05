using KeZhan.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using TiKuBll;
using TiKuBll.Model;

namespace KeZhan.Controllers
{
    public class TICResponseController : Controller
    {
        //
        // GET: /TICResponse/
        [HttpPost]
        public void SaasResponse()
        {
            String responstData = "";
            var stream = Request.InputStream;
            stream.Position = 0;
            using (var streamReader = new StreamReader(stream, Encoding.UTF8))
            {
                responstData = streamReader.ReadToEndAsync().Result;
                WriteFile(responstData);
            }


            JavaScriptSerializer serializer = new JavaScriptSerializer();
            SaasClassOverModel model = serializer.Deserialize<SaasClassOverModel>(responstData);
            if (model.@event == "class_over")
            {
                GroupUserBll.UpdateGroup(model.data.class_id, "closegroup");
            }
            else if (model.@event == "class_begin")
            {

            }
               


            Response.Write("{\"error_code\":0}");
        }


        /// <summary>
        /// IM回调
        /// </summary>
        /// <param name="SdkAppid">App 在云通信 IM 分配的应用标识</param>
        /// <param name="CallbackCommand">回调命令字</param>
        /// <param name="contenttype">可选，通常值为 JSON</param>
        /// <param name="ClientIP">客户端 IP 地址</param>
        /// <param name="OptPlatform">客户端平台，对应不同的平台类型，可能的取值有：RESTAPI（使用 REST API 发送请求）、Web（使用 Web SDK 发送请求）、Android、iOS、Windows、Mac、Unkown（使用未知类型的设备发送请求）</param>
        /// <returns></returns>


        [HttpPost]
        public JsonResult IMResponse(string SdkAppid, string CallbackCommand, string contenttype, string ClientIP, string OptPlatform)
        {
            String responstData = "";
            var stream = Request.InputStream;
            stream.Position = 0;
            using (var streamReader = new StreamReader(stream, Encoding.UTF8))
            {
                responstData = streamReader.ReadToEndAsync().Result;
                WriteFile(responstData);
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            //在线状态变更  	State.StateChange
            if (CallbackCommand == "State.StateChange")
            {
                StateChange Model = serializer.Deserialize<StateChange>(responstData);
                if (Model.Info.Action == "Login")
                {
                    GroupUserBll.UpdateGroupUserInfo(Model.Info.To_Account, "online");
                }
                else if (Model.Info.Action == "Logout")
                {
                    GroupUserBll.UpdateGroupUserInfo(Model.Info.To_Account, "offline");
                }
                //{
                //    "CallbackCommand": "State.StateChange",
                //    "Info": {
                //        "Action": "Logout",
                //        "To_Account": "testuser316",
                //        "Reason": "Unregister"
                //    }
                //}
            }
            //创建群组前 
            if (CallbackCommand == "Group.CallbackBeforeCreateGroup")
            {
                CallbackBeforeCreateGroup Model = serializer.Deserialize<CallbackBeforeCreateGroup>(responstData);

                //{
                //    "CallbackCommand": "Group.CallbackBeforeCreateGroup", // 回调命令
                //    "Operator_Account": "leckie", // 操作者
                //    "Owner_Account": "leckie", // 群主
                //    "Type": "Public", // 群组类型
                //    "Name": "MyFirstGroup", // 群组名称
                //    "CreatedNum": 123, //该用户已创建的同类的群组个数
                //    "MemberList": [ // 初始成员列表
                //        {
                //            "Member_Account": "bob"
                //        },
                //        {
                //            "Member_Account": "peter"
                //        }
                //    ]
                //}
            }

            //创建群组后 Group.CallbackAfterCreateGroup
            if (CallbackCommand == "Group.CallbackAfterCreateGroup")
            {
                CallbackAfterCreateGroup model = serializer.Deserialize<CallbackAfterCreateGroup>(responstData);
                //添加课堂记录
                if (!model.GroupId.Contains("chat"))
                {
                    GroupModel group = GroupUserBll.GetGroup(model.GroupId);

                    if (group == null)
                    {
                        CourseModel course = ClassRoomBll.GetCourseByClassID(model.GroupId);
                        int i = GroupUserBll.InsertGroup(model.GroupId, model.Name, course.fID, model.Owner_Account);
                    }
                }
                //{
                //    "CallbackCommand": "Group.CallbackAfterCreateGroup", // 回调命令
                //    "GroupId" : "@TGS#2J4SZEAEL",
                //    "Operator_Account": "group_root", // 操作者
                //    "Owner_Account": "leckie", // 群主
                //    "Type": "Public", // 群组类型
                //    "Name": "MyFirstGroup", // 群组名称
                //    "MemberList": [ // 初始成员列表
                //        {
                //            "Member_Account": "bob"
                //        },
                //        {
                //            "Member_Account": "peter"
                //        }
                //    ],
                //    "UserDefinedDataList": [ //用户建群时的自定义字段
                //        {
                //            "Key": "UserDefined1",
                //            "Value": "hello"
                //        },
                //        {
                //            "Key": "UserDefined2",
                //            "Value": "world"
                //        }
                //    ]
                //}
            }
            //新人入群 Group.CallbackAfterNewMemberJoin
            if (CallbackCommand == "Group.CallbackAfterNewMemberJoin")
            {
                CallbackAfterNewMemberJoin model = serializer.Deserialize<CallbackAfterNewMemberJoin>(responstData);
                if (!model.GroupId.Contains("chat"))
                {
                    foreach (TICMember m in model.NewMemberList)
                    {

                        int i = GroupUserBll.InsertGroupUserInfo(m.Member_Account, model.GroupId, "Student");
                    }
                }


                //{
                //    "CallbackCommand": "Group.CallbackAfterNewMemberJoin", // 回调命令
                //    "GroupId" : "@TGS#2J4SZEAEL",
                //    "Type": "Public", // 群组类型
                //    "JoinType": "Apply", // 入群方式：Apply（申请入群）；Invited（邀请入群）
                //    "Operator_Account": "leckie", // 操作者成员
                //    "NewMemberList": [ // 新入群成员列表
                //        {
                //            "Member_Account": "jared"
                //        },
                //        {
                //            "Member_Account": "tommy"
                //        }
                //    ]
                //}
            }
            //群成员离开 Group.CallbackAfterMemberExit
            if (CallbackCommand == "Group.CallbackAfterMemberExit")
            {
                CallbackAfterMemberExit model = serializer.Deserialize<CallbackAfterMemberExit>(responstData);
                foreach (TICMember m in model.ExitMemberList)
                {
                    int i = GroupUserBll.UpdateGroupUserInfo(m.Member_Account, model.GroupId, "offline");
                }
                //{
                //    "CallbackCommand": "Group.CallbackAfterMemberExit", // 回调命令
                //    "GroupId": "@TGS#2J4SZEAEL", // 群组 ID
                //    "Type": "Public", // 群组类型
                //    "ExitType": "Kicked", // 成员离开方式：Kicked-被踢；Quit-主动退群
                //    "Operator_Account": "leckie", // 操作者
                //    "ExitMemberList": [ // 离开群的成员列表
                //        {
                //            "Member_Account": "jared"
                //        },
                //        {
                //            "Member_Account": "tommy"
                //        }
                //    ]
                //}
            }
            //群组解散 Group.CallbackAfterGroupDestroyed
            if (CallbackCommand == "Group.CallbackAfterGroupDestroyed")
            {
                CallbackAfterGroupDestroyed model = serializer.Deserialize<CallbackAfterGroupDestroyed>(responstData);
                GroupUserBll.UpdateGroup(model.GroupId, "closegroup");
               
                //{
                //    "CallbackCommand": "Group.CallbackAfterGroupDestroyed", // 回调命令
                //    "GroupId" : "@TGS#2J4SZEAEL",
                //    "Type": "Public", // 群组类型
                //    "Owner_Account": "leckie", // 群主
                //    "Name": "MyFirstGroup", // 群组名称
                //    "MemberList" : [ //被解散的群组中的成员
                //        {
                //            "Member_Account": "leckie"
                //        },
                //        {
                //            "Member_Account": "peter"
                //        },
                //        {
                //            "Member_Account": "bob"
                //        }
                //    ]
                //}
            }

            JsonResult jr = new JsonResult();
            //{
            //    "ActionStatus": "OK",
            //    "ErrorInfo": "",
            //    "ErrorCode": 0 // 忽略应答结果
            //}
            jr.Data = "{\"ActionStatus\": \"OK\",\"ErrorInfo\": \"\",\"ErrorCode\": 0}";
            jr.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jr;
        }



        public static void WriteFile(string content)
        {
            try
            {
                StreamWriter sw = new StreamWriter(@"\TICResponseLog\" + DateTime.Now.ToString("yyyyMMdd") + ".txt", true);
                sw.WriteLine(content + "\r\n"+DateTime.Now.ToString()+"----------------------------------------\r\n");
                sw.Close();//写入
            }
            catch
            {

            }


        }
    }
}
