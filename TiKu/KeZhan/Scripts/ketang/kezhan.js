Vue.use(Toasted);
var app = new Vue({
    el: '#app_box',
    data() {
    return {
    step: 'first',
    pushModel: 0, // 1  自动推流 0 手动推流
    account: '',
userID: sessionStorage.getItem('IIC_USERNAME'),
sdkAppId: '1400178589',
userSig: '',
nickName: sessionStorage.getItem('IIC_NICKNAME'),
headImage:'',
roomInfo: '',
roomID: Math.floor(Math.random() * 1000000000),
isTeacher: 1,    
enableHand: false,
enableCamera: true,
enableMic: true,
msgs: [],
showDialog: false,
dialogTitle: '',
dialogContent: '',
dialogAction: '',
dialogInfo: [],
groupuser:[],
isPushing: 0, // 是否正在推流
isPushCamera: 0, // 是否推摄像头流
ishand:false,
devices: {
    camera: [],
    mic: []
},

cameraIndex: 0,
    micIndex: 0,

imMsg: {
    common: {},
    custom: {}
},

boardData: {
        data: {
            current: null,
            list: []
        },
    page: {
            current: 0,
            total: 0
    }
},

loginConfig: {
        identifier: null,
        identifierNick: null,
        userHeadImg: null,
        userSig: null,
        sdkAppId: null
},

webrtcConfig: {
        closeLocalMedia: true,
        audio: true,
        video: true,
        role: null
},

boardConfig: {
        id: null,
        canDraw: null,
        color: null,
        globalBackgroundColor: null
},

cosConfig: {
        appid: null,
        bucket: null,
        region: null,
        sign: null
},

remoteVideos: {},

boardFileGroup: [], // 白板文件组
    currentFile: null, // 当前白板组
    }
    },

mounted() {

},

methods: {
    // 创建或者进入课堂
    start() {
        if (!this.roomID) {
            this.showErrorTip('房间号不能为空');
            return;
        }
        this.step = 'second';
        this.init();
    },
  initLogin() {
      if (!this.account) {
          this.showErrorTip('账号不能为空');
          return;
      }
      this.step = 'two';
  },
  initLogin2(username,sig,role,roomid,nick,image) {
      this.account = username;
      this.userSig=sig;
      this.roomID=roomid;
      this.nickName=nick;
      this.headImage=image;
      if(role=="Teacher")
      {
          this.isTeacher=1;
          this.pushModel=1;
      }else{
          this.isTeacher=0;
          this.pushModel=0;
          this.enableCamera=false;
          this.enableMic=false;
      }
      this.step = 'second';
      this.init();

   
  },
  techerStart(){
      this.isTeacher=1;
      this.pushModel=1;
      if (!this.roomID) {
          this.showErrorTip('房间号不能为空');
          return;
      }
      this.step = 'second';
      this.init();
  },
  studentStart(){
      this.isTeacher=0;
      this.pushModel=0;
      this.enableCamera=false;
      this.enableMic=false;
      if (!this.roomID) {
          this.showErrorTip('房间号不能为空');
          return;
      }
      this.step = 'second';
      this.init();
  },

  initData() {


      this.msgs = [];
      
      this.groupuser = [];
      this.devices = {
          camera: [],
          mic: []
      };

      this.cameraIndex = 0;
      this.micIndex = 0;

      this.imMsg = {
          common: {
              data: '',
              toUser: ''
          },
          custom: {
              data: '',
              desc: '',
              ext: '',
              toUser: ''
          }
      };


      this.loginConfig = {
          identifier: this.account,
          identifierNick: '用户昵称' + this.account,
          userSig: this.userSig,
          sdkAppId: this.sdkAppId,
          accountType: 1
      };
      localStorage.setItem('IIC_USERID', this.account);

      this.webrtcConfig = {
          closeLocalMedia: true,
          audio: true,
          video: true,
          role: 'user'
      };

      this.boardConfig = {
          id: 'paint_box',
          canDraw: this.isTeacher, // 老师能画，学生不能画
          globalBackgroundColor: '#ffffff',
          color: '#ff0000',
          boardMode: 0, // 白板模式
          textSize: 24,
          textColor: '#ff0000'
      }

      this.enableCamera = true;
      this.enableMic = true;
  },

  init() {
      this.initData();
      this.ticSdk = null;
      this.ticSdk = new TICSDK({
          debug: location.href.indexOf('test=1') > -1
      });

      this.ticSdk.init();
      this.initEvent();
      this.ticSdk.login(this.loginConfig);
  },

  initEvent() {
      this.ticSdk.on(TICSDK.CONSTANT.EVENT.IM.CONNECTION_EVENT, res => {
          switch (res.ErrorCode) {
          case webim.CONNECTION_STATUS.ON:
      console.log('连接状态正常...');
      break;
        case webim.CONNECTION_STATUS.OFF:
          this.showErrorTip('IM 连接已断开，无法收到新消息，请检查下你的网络是否正常');
          console.error('连接已断开，无法收到新消息，请检查下你的网络是否正常');
          break;
        default:
          this.showErrorTip('未知连接状态,status=' + res.ErrorCode);
          console.error('未知连接状态,status=' + res.ErrorCode);
          break;
}
});

// IM登录成功后，老师创建课堂，学生则加入课堂
this.ticSdk.on(TICSDK.CONSTANT.EVENT.IM.LOGIN_SUCC, res => {
    this.showTip('IM 登录成功');

if (this.isTeacher) {
    // 老师就创建课堂
    this.ticSdk.createClassroom({
        roomID: this.roomID,
        roomType: 'Public'
    });
} else { // 如果是学生
    // 有了课堂后就直接加入
    this.roomID && this.ticSdk.joinClassroom(this.roomID, this.webrtcConfig, this.boardConfig);
}




});

this.ticSdk.on(TICSDK.CONSTANT.EVENT.IM.LOGIN_ERROR, err => {
    this.showErrorTip('IM 登录失败');
console.error('IM 登录失败', err);
this.step = 'first';
});

this.ticSdk.on(TICSDK.CONSTANT.EVENT.IM.GROUP_IS_ALREADY_USED_ERROR, err => {
    this.showErrorTip('房间已经被使用，请换其他房间');
console.error('房间已经被使用，请换其他房间', err);
this.step = 'first';
});

// 老师创建了课堂后，则需要进入课堂
this.ticSdk.on(TICSDK.CONSTANT.EVENT.TIC.CREATE_CLASS_ROOM_SUCC, (res) => {
    console.log('TICSDK.CONSTANT.EVENT.TIC.CREATE_CLASS_ROOM_SUCC');
this.showTip('创建课堂成功');

// 如果是老师
if (this.isTeacher) {
    this.roomID && this.ticSdk.joinClassroom(this.roomID, this.webrtcConfig, this.boardConfig);
}
});

this.ticSdk.on(TICSDK.CONSTANT.EVENT.BOARD.HISTROY_DATA_COMPLETE, () => {
    this.showTip('历史数据加载完成');
console.log('TICSDK.CONSTANT.EVENT.BOARD.HISTROY_DATA_COMPLETE');
this.boardFileGroup = board.getAllFileInfo();
this.currentFile = board.getCurrentFile();
});

this.ticSdk.on(TICSDK.CONSTANT.EVENT.TIC.CREATE_CLASS_ROOM_ERROR, res => {
    this.showErrorTip('创建课堂失败');
console.log('TICSDK.CONSTANT.EVENT.TIC.CREATE_CLASS_ROOM_ERROR');
this.step = 'first';
});

this.ticSdk.on(TICSDK.CONSTANT.EVENT.WEBRTC.INIT_SUCC, res => {
    this.showTip('WebRTC初始化成功');
console.log('TICSDK.CONSTANT.EVENT.WEBRTC.INIT_SUCC');

});

this.ticSdk.on(TICSDK.CONSTANT.EVENT.WEBRTC.INIT_ERROR, err => {
    console.log('TICSDK.CONSTANT.EVENT.WEBRTC.INIT_ERROR');
this.step = 'first';
this.showErrorTip('WebRTC初始化失败');
});
this.ticSdk.on(TICSDK.CONSTANT.EVENT.WEBRTC.LOCAL_STREAM_ADD, data => {
    document.getElementById('local_video').srcObject = data.stream;
this.isPushing = 1; // 正在推流
this.showTip('WebRTC接收到本地流');
});
this.ticSdk.on(TICSDK.CONSTANT.EVENT.WEBRTC.REMOTE_STREAM_UPDATE, data => {
    this.$set(this.remoteVideos, data.videoId, data);

//stream,videoId,type,ssrcState,openId,videoType,userId
this.$nextTick(() => {
    if (document.getElementById(data.videoId)) {
    document.getElementById(data.videoId).srcObject = data.stream;
document.getElementById(data.videoId).controls = true;
document.getElementById(data.videoId).mounted = true;
}
});
this.showTip('WebRTC接收到远端流');
});

this.ticSdk.on(TICSDK.CONSTANT.EVENT.WEBRTC.REMOTE_STREAM_REMOVE, data => {
    this.$delete(this.remoteVideos, data.videoId);
this.showTip('WebRTC 远端流断开');
});

this.ticSdk.on(TICSDK.CONSTANT.EVENT.WEBRTC.PEER_CONNECTION_ADD, data => {
    console.log('WebRTC PEER_CONNECTION_ADD');
this.showTip('WebRTC PEER_CONNECTION_ADD');
});

this.ticSdk.on(TICSDK.CONSTANT.EVENT.WEBRTC.ERROR_NOTIFY, data => {
    console.log('WebRTC ERROR_NOTIFY');
this.showTip('音视频链接断开，请重新开始进入课堂');
});

this.ticSdk.on(TICSDK.CONSTANT.EVENT.WEBRTC.WEBSOCKET_NOTIFY, data => {
    console.log('WebRTC WEBSOCKET_NOTIFY');
this.showTip('WebRTC WEBSOCKET_NOTIFY');
});

this.ticSdk.on(TICSDK.CONSTANT.EVENT.WEBRTC.STREAM_NOTIFY, data => {
    console.log('WebRTC STREAM_NOTIFY');
this.showTip('WebRTC STREAM_NOTIFY');
});

//
this.ticSdk.on(TICSDK.CONSTANT.EVENT.TIC.ROOMID_NOT_FOUND, data => {
    this.showTip('学生进入房间请输入房间号');
});

this.ticSdk.on(TICSDK.CONSTANT.EVENT.TIC.JOIN_CLASS_ROOM_SUCC, data => {
    this.showTip('加入课堂成功');

var profile_item = [
     {
         "Tag": "Tag_Profile_IM_Nick",
         "Value": this.nickName
     },
     {
         "Tag": "Tag_Profile_IM_Image",
         "Value": this.headImage
     }
];
var options = {
    'ProfileItem': profile_item
};

webim.setProfilePortrait(
        options,
        function (resp) {
            this.showTip('设置个人资料成功');
            //alert('设置个人资料成功');
        },
        function (err) {
            alert("设置头像出错"+err.ErrorInfo);
        }
);

setTimeout(()=>{
    this.groupuser = [];

//读取群组成员
var options = {
    'GroupId': this.roomID,
    'Offset': 0, //必须从0开始
    'Limit': 60,
    'MemberInfoFilter': [
        'Account',
        'Role',
        'JoinTime',
        'LastSendMsgTime',
        'ShutUpUntil'
    ]
};
webim.getGroupMemberInfo(
        options,
        function (resp) {   
            var userList=[];
            for (var i in resp.MemberList) {

                var account = resp.MemberList[i].Member_Account;
                //app.groupuser.push({
                //    content: account
                //});
                userList.push(account);
            }  
            var tag_list = [
       "Tag_Profile_IM_Nick",//昵称
       "Tag_Profile_IM_Image"//头像
            ];
            var options = {
                'To_Account': userList,
                'TagList': tag_list
            };
            webim.getProfilePortrait(
                    options,
                    function (resp) {
                        if (resp.UserProfileItem && resp.UserProfileItem.length > 0) {
                            for (var i in resp.UserProfileItem) {
                                var to_account = resp.UserProfileItem[i].To_Account;
                                var nick = null, gender = null, allowType = null,imageUrl=null;
                                for (var j in resp.UserProfileItem[i].ProfileItem) {
                                    switch (resp.UserProfileItem[i].ProfileItem[j].Tag) {
                                        case 'Tag_Profile_IM_Nick':
                                            nick = resp.UserProfileItem[i].ProfileItem[j].Value;
                                            break;                                      
                                        case 'Tag_Profile_IM_AllowType':
                                            switch (resp.UserProfileItem[i].ProfileItem[j].Value) {
                                                case 'AllowType_Type_AllowAny':
                                                    allowType = '允许任何人';
                                                    break;
                                                case 'AllowType_Type_NeedConfirm':
                                                    allowType = '需要确认';
                                                    break;
                                                case 'AllowType_Type_DenyAny':
                                                    allowType = '拒绝任何人';
                                                    break;
                                                default:
                                                    allowType = '需要确认';
                                                    break;
                                            }
                                            break;
                                        case 'Tag_Profile_IM_Image':
                                            imageUrl = resp.UserProfileItem[i].ProfileItem[j].Value;
                                            break;
                                    }
                                }
                                app.groupuser.push({
                                    'content': to_account,
                                    'Nick': webim.Tool.formatText2Html(nick),
                                    'Image': "https://aizhu-ducation.oss-cn-hangzhou.aliyuncs.com/"+imageUrl+"?x-oss-process=style/head"
                                });
                            }
                        }
                    },
                    function (err) {
                        alert("获取列表出错1"+err.ErrorInfo);
                    }
            );
        },
        function (err) {
            alert("获取列表出错2"+err.ErrorInfo);
        }
);

},3000)
window.board = app.ticSdk.getBoardInstance();
window.WebRTC = this.ticSdk.getWebRTCInstance();

// 如果是主动推流
if (this.pushModel === 1) {
    this.startRTC();
}

//获取摄像头，麦克风
this.getCameraDevices()
this.getMicDevices()

});

this.ticSdk.on(TICSDK.CONSTANT.EVENT.TIC.JOIN_CLASS_ROOM_ERROR, data => {
    this.showErrorTip('加入课堂失败,请确定该课堂是否已经由老师创建');
this.step = 'first';
});


this.ticSdk.on(TICSDK.CONSTANT.EVENT.IM.GROUP_SYSTEM_NOTIFYS, imEvent => {
    if (imEvent.event_type === 5) { // 群被解散
    this.ticSdk.quitClassroom();
this.showTip(`老师解散了课堂`);
} else if (imEvent.event_type === 8) {
    this.showTip(`退出了课堂`);
}
});

// 接收到聊天群组消息
this.ticSdk.on(TICSDK.CONSTANT.EVENT.IM.RECEIVE_CHAT_ROOM_MSG, msgs => {
    console.log('TICSDK.CONSTANT.EVENT.IM.RECEIVE_CHAT_ROOM_MSG');
msgs.elems.forEach(msg => {
    var content = msg.getContent();
if (msgs.getFromAccount() === '@TIM#SYSTEM') { // 接收到系统消息
    var opType = content.getOpType(); // 通知类型
    if (opType === webim.GROUP_TIP_TYPE.JOIN) { // 加群通知
        this.msgs.push({
            send: '群消息提示：',
            content: content.getOpUserId() + '进群了'
        });
        
        setTimeout(()=>{
            if(this.account!=content.getOpUserId()){
            var userList=[];
        userList.push(content.getOpUserId());
        var tag_list = [
  "Tag_Profile_IM_Nick",//昵称
  "Tag_Profile_IM_Image"//头像
        ];
        var options = {
            'To_Account':userList ,
            'TagList': tag_list
        };
        webim.getProfilePortrait(
                options,
                function (resp) {
                    if (resp.UserProfileItem && resp.UserProfileItem.length > 0) {
                        for (var i in resp.UserProfileItem) {
                            var to_account = resp.UserProfileItem[i].To_Account;
                            var nick = null, gender = null, allowType = null,imageUrl=null;
                            for (var j in resp.UserProfileItem[i].ProfileItem) {
                                switch (resp.UserProfileItem[i].ProfileItem[j].Tag) {
                                    case 'Tag_Profile_IM_Nick':
                                        nick = resp.UserProfileItem[i].ProfileItem[j].Value;
                                        break;                                      
                                    case 'Tag_Profile_IM_AllowType':
                                        switch (resp.UserProfileItem[i].ProfileItem[j].Value) {
                                            case 'AllowType_Type_AllowAny':
                                                allowType = '允许任何人';
                                                break;
                                            case 'AllowType_Type_NeedConfirm':
                                                allowType = '需要确认';
                                                break;
                                            case 'AllowType_Type_DenyAny':
                                                allowType = '拒绝任何人';
                                                break;
                                            default:
                                                allowType = '需要确认';
                                                break;
                                        }
                                        break;
                                    case 'Tag_Profile_IM_Image':
                                        imageUrl = resp.UserProfileItem[i].ProfileItem[j].Value;
                                        break;
                                }
                            }
                            app.groupuser.push({
                                'content': to_account,
                                'Nick': webim.Tool.formatText2Html(nick),
                                'Image': "https://aizhu-ducation.oss-cn-hangzhou.aliyuncs.com/"+imageUrl+"?x-oss-process=style/head"
                            });
                        }
                    }
                },
                function (err) {
                    alert("新进群错误"+err.ErrorInfo);
                }
        );
        //this.groupuser.push({
        //    content: content.getOpUserId()
        //});
    }
},2000);
} else if (opType === webim.GROUP_TIP_TYPE.QUIT) { // 退群通知
    this.msgs.push({
        send: '群消息提示：',
        content: content.getOpUserId() + '退群了'
    });
    for(var i=0;i<this.groupuser.length;i++){
        if( this.groupuser[i].content==content.getOpUserId()){
            this.groupuser.splice(i,1);
        }
    }
} else if (opType === webim.GROUP_TIP_TYPE.KICK) { // 踢人通知

} else if (opType === webim.GROUP_TIP_TYPE.SET_ADMIN) { // 设置管理员通知

} else if (opType === webim.GROUP_TIP_TYPE.CANCEL_ADMIN) { // 取消管理员通知

} else if (opType === webim.GROUP_TIP_TYPE.MODIFY_GROUP_INFO) { // 群资料变更

} else if (opType === webim.GROUP_TIP_TYPE.MODIFY_MEMBER_INFO) { //群成员资料变更

}
} else { // 接收到群聊天消息
    var type = msg.getType();
    if (type === 'TIMTextElem') {
        this.msgs.push({
            send: msgs.getFromAccount() + '：',
            content: content.getText()
        });
        var div = document.getElementById('chat-msg-list');
        
        div.scrollTop = div.scrollHeight;
    } else if (type === 'TIMCustomElem') {
        //    this.msgs.push({
        //        send: msgs.getFromAccount() + '：',
        //        content: `data: ${content.getData()} desc: ${content.getDesc()} ext: ${content.getExt()}`
           
        //});
        //老师开放举手
        if (content.getExt() == 'invite_interact_notify') {
            if (content.getData() == 0) {
                this.showTip("老师已开放举手");
                this.ishand=true
                return;
            }else if (content.getData() == 1) {
                this.showTip("老师已关闭举手");
                this.ishand=false
                return;
            }
        }
    }
}
});
});

// 接收到C2C消息
this.ticSdk.on(TICSDK.CONSTANT.EVENT.IM.RECEIVE_C2C_MSG, msgs => {
    console.log('TICSDK.CONSTANT.EVENT.IM.RECEIVE_C2C_MSG');
msgs.elems.forEach(msg => {
    var content = msg.getContent();
var type = msg.getType();
if (type === 'TIMTextElem') {
    this.msgs.push({
        send: msgs.getFromAccount() + '：',
        content: content.getText()
    });
   
    var div = document.getElementById('chat-msg-list');
    div.scrollTop = div.scrollHeight;
} else if (type === 'TIMCustomElem') {
    //    this.msgs.push({
    //        send: msgs.getFromAccount() + '：',
    //        content: `data: ${content.getData()} desc: ${content.getDesc()} ext: ${content.getExt()}`
    //});
    if(msgs.getFromAccount()==this.account){
        return;
    }
    if(content.getExt()=="apply_permission_notify"){
        console.log('收到学生请求');
        //this.dialogInfo = {
        //    "conf_id": content.getDesc(),
        //    "member": msgs.getFromAccount(),
        //    "permission": content.getData()
        //}
        for(var i=0;i<this.dialogInfo.length;i++){
            if( this.dialogInfo[i].member==msgs.getFromAccount()){
                return;
            }
        }
        
        this.dialogInfo.push({
            "conf_id": content.getDesc(),
            "member": msgs.getFromAccount(),
            "permission": content.getData()
        });

        this.triggerDialog({
            title: '收到学生连麦请求？',
            content: '收到学生' + msgs.getFromAccount() + '连麦请求，是否接受？',
            action: ''
        });
    }else if (content.getExt() == 'invite_interact_notify') {
        console.log('收到老师请求');
        this.showTip("收到老师互动请求");
        this.triggerDialog({
            title: '是否接受老师连麦？',
            content: '收到老师连麦请求，是否接受？',
            action: 'acceptTeacher'
        })
        setTimeout(()=>{
            this.showDialog = false;
    },10000)
    } else if (content.getExt() == 'grant_permission_notify') {
        console.log('老师批准请求');
        if (content.getData() == 0) {
            // self.$toastr.success('被老师下麦', '提示');
            
            this.toggleCamera();
            return;
        } else if (content.getData() == 1) {
            // self.$toastr.success('被老师关闭声音', '提示');
            this.toggleMic();
            return;
        }  else if (content.getData() == 2) {
            // self.$toastr.success('被老师关闭声音', '提示');
            this.showTip("被老师停止互动");
            this.stopPush();
            return;
        }else if (content.getData() == 4) {
            // self.$toastr.success('被老师打开白板涂鸦', '提示');
            this.showTip("被老师打开白板涂鸦");
            this.ticSdk.getBoardInstance().setCanDraw(true);
            this.boardConfig = {
                id: 'paint_box',
                canDraw: 1, 
                globalBackgroundColor: '#ffffff',
                color: '#ff0000',
                boardMode: 0, // 白板模式
                textSize: 24,
                textColor: '#ff0000'
            }
            return;
        } else if (content.getData() == 5) {
            // self.$toastr.success('被老师关闭白板涂鸦', '提示');
            
            this.showTip("被老师关闭白板涂鸦");
            this.ticSdk.getBoardInstance().setCanDraw(false);
            this.boardConfig = {
                id: 'paint_box',
                canDraw: 0, //
                globalBackgroundColor: '#ffffff',
                color: '#ff0000',
                boardMode: 0, // 白板模式
                textSize: 24,
                textColor: '#ff0000'
            }
            return;
        }  else if (content.getData() == 6) {
            //self.$toastr.success('老师批准请求', '提示');

            this.startRTC();
        }
    } 
}
});
});

// 接收到普通消息
this.ticSdk.on(TICSDK.CONSTANT.EVENT.IM.MSG_NOTIFY, msgs => {
    console.log('TICSDK.CONSTANT.EVENT.IM.MSG_NOTIFY');
});

this.ticSdk.on(TICSDK.CONSTANT.EVENT.BOARD.ADD_BOARD, data => {
    this.proBoardData(data);
console.log('TICSDK.CONSTANT.EVENT.BOARD.ADD_BOARD', '白板增加一页');
});

this.ticSdk.on(TICSDK.CONSTANT.EVENT.BOARD.DELETE_BOARD, data => {
    this.proBoardData(data);
console.log('TICSDK.CONSTANT.EVENT.BOARD.DELETE_BOARD', '白板删除一页');
});

this.ticSdk.on(TICSDK.CONSTANT.EVENT.BOARD.SWITCH_BOARD, data => {
    this.proBoardData(data);
console.log('TICSDK.CONSTANT.EVENT.BOARD.SWITCH_BOARD', '白板切换');
});

this.ticSdk.on(TICSDK.CONSTANT.EVENT.BOARD.ADD_GROUP, gid => {
    console.log('TICSDK.CONSTANT.EVENT.BOARD.ADD_GROUP', '增加白板组');
});

this.ticSdk.on(TICSDK.CONSTANT.EVENT.BOARD.DELETE_GROUP, gid => {
    console.log('TICSDK.CONSTANT.EVENT.BOARD.DELETE_GROUP', '删除白板组');
});

this.ticSdk.on(TICSDK.CONSTANT.EVENT.BOARD.SWITCH_GROUP, gid => {
    console.log('TICSDK.CONSTANT.EVENT.BOARD.SWITCH_GROUP', '切换白板组');
});

this.ticSdk.on(TICSDK.CONSTANT.EVENT.BOARD.REAL_TIME_DATA, data => {
    console.log('TICSDK.CONSTANT.EVENT.BOARD.REAL_TIME_DATA', '接收到白板实时数据', data);
});

this.ticSdk.on(TICSDK.CONSTANT.EVENT.BOARD.ADD_DATA_ERROR, data => {
    console.log('TICSDK.CONSTANT.EVENT.BOARD.ADD_DATA_ERROR', '接收到白板其他用户数据有错误');
});

this.ticSdk.on(TICSDK.CONSTANT.EVENT.BOARD.VERIFY_SDK_SUCC, () => {
    console.log('白板SDK鉴权通过');
});

this.ticSdk.on(TICSDK.CONSTANT.EVENT.BOARD.VERIFY_SDK_ERROR, () => {
    console.error('白板SDK鉴权不通过');
this.showTip('白板SDK鉴权不通过');
});

this.ticSdk.on(TICSDK.CONSTANT.EVENT.BOARD.IMG_START_LOAD, (data) => {
    console.log('TICSDK.CONSTANT.EVENT.BOARD.ADD_DATA_ERROR', '开始加载图片', data);
this.showTip('开始加载图片');
});

this.ticSdk.on(TICSDK.CONSTANT.EVENT.BOARD.IMG_LOAD, (data) => {
    console.log('TICSDK.CONSTANT.EVENT.BOARD.ADD_DATA_ERROR', '图片加载成功', data);
this.showTip('图片加载成功');
});

this.ticSdk.on(TICSDK.CONSTANT.EVENT.BOARD.IMG_ERROR, (data) => {
    console.log('TICSDK.CONSTANT.EVENT.BOARD.ADD_DATA_ERROR', '图片加载失败', data);
this.showTip('图片加载失败');
});

this.ticSdk.on(TICSDK.CONSTANT.EVENT.BOARD.IMG_ABORT, (data) => {
    console.log('TICSDK.CONSTANT.EVENT.BOARD.ADD_DATA_ERROR', '图片中断加载', data);
this.showTip('图片中断加载');
});

this.ticSdk.on(TICSDK.CONSTANT.EVENT.COS.TASK_READY, data => {
    console.log('TICSDK.CONSTANT.EVENT.COS.TASK_READY', '上传任务创建时的回调函数，返回一个 taskId');
});

this.ticSdk.on(TICSDK.CONSTANT.EVENT.COS.HASH_PROGRESS, data => {
    console.log('TICSDK.CONSTANT.EVENT.COS.HASH_PROGRESS', '计算文件 MD5 值的进度回调函数');
});


this.ticSdk.on(TICSDK.CONSTANT.EVENT.COS.PROGRESS, data => {
    console.log('TICSDK.CONSTANT.EVENT.COS.PROGRESS', '上传文件的进度回调函数 data.percent:', data);
this.showTip(`上传进度：${Math.floor(data.percent * 100)}%`);
});

this.ticSdk.on(TICSDK.CONSTANT.EVENT.COS.GET_SIGN_ERROR, data => {
    this.showTip(`获取sigin错误`);
});

this.ticSdk.on(TICSDK.CONSTANT.EVENT.COS.GET_SIGN_SUCCESS, data => {
    this.showTip(`获取sigin成功`);
});

this.ticSdk.on(TICSDK.CONSTANT.EVENT.COS.UPLOAD_SUCCESS, data => {
    this.showTip(`上传成功`);
this.showTip(`文件上传完成，正在获取文件总页数~`);
});

this.ticSdk.on(TICSDK.CONSTANT.EVENT.COS.UPLOAD_ERROR, data => {
    this.showTip(`上传失败`);
});

this.ticSdk.on(TICSDK.CONSTANT.EVENT.TIC.QUIT_CLASS_ROOM_SUCC, data => {
    this.step = 'first';
this.showTip(`退出课堂成功`);
});

this.ticSdk.on(TICSDK.CONSTANT.EVENT.TIC.QUIT_CLASS_ROOM_ERROR, data => {
    this.showErrorTip(`退出课堂失败`);
});

this.ticSdk.on(TICSDK.CONSTANT.EVENT.TIC.DESTROY_CLASS_ROOM_SUCC, () => {
    this.step = 'first';
this.showTip(`销毁课堂成功`);
});

this.ticSdk.on(TICSDK.CONSTANT.EVENT.TIC.DESTROY_CLASS_ROOM_ERROR, () => {
    this.showErrorTip(`销毁课堂失败`);
});

this.ticSdk.on(TICSDK.CONSTANT.EVENT.IM.KICKED, () => {
    alert(`其他地方登录，被T了`);
this.step = 'first';
});
},

// 启动推流(推摄像头)
startRTC() {
    // 获取webrtc实例
    var WebRTC = this.ticSdk.getWebRTCInstance();
    WebRTC.getLocalStream({
        audio: true,
        video: true,
        attributes: {
            width: 640,
            height: 480
        }
    }, (data) => {
        this.isPushCamera = true;
    if (WebRTC.global.localStream && WebRTC.global.localStream.active) {
        WebRTC.updateStream({
            role: 'screen',
            stream: data.stream
        }, () => {
            // 成功
        }, (error) => {
            this.showErrorTip(`更新流失败，${error}`);
    });
} else {
          WebRTC.startRTC({
              stream: data.stream,
              role: 'user'
          }, (data) => {
              // 成功
          }, (error) => {
              this.showErrorTip(`推流失败, ${error}`);
});
}
}, (error) => {
    this.showErrorTip(`获取本地流失败, ${JSON.stringify(error)}`);
});
},

stopPush() {
    var WebRTC = this.ticSdk.getWebRTCInstance();
    WebRTC.stopRTC({}, () => {
        this.isPushing = 0;
    document.querySelector('#local_video').srcObject = null;
});
},

/**
 * 推屏幕分享
 */
pushScreen() {
    var WebRTC = this.ticSdk.getWebRTCInstance();
    
    WebRTC.getLocalStream({
        screen: true,
        screenSources: 'screen, window, tab, audio',
        attributes: {
            width: 1920,
            height: 1080,
            frameRate: 10
        }
    }, (data) => {
        this.isPushCamera = false;
    if (WebRTC.global.localStream && WebRTC.global.localStream.active) {
        WebRTC.updateStream({
            role: 'screen',
            stream: data.stream
        }, () => {
            // 成功
        }, (error) => {
            this.showErrorTip(`更新流失败，${error}`);
    });
} else {
          WebRTC.startRTC({
              stream: data.stream,
              role: 'user'
          }, (data) => {
              // 成功
          }, (error) => {
              this.showErrorTip(`推流失败, ${error}`);
});
}
}, (error) => {
    this.showErrorTip(`获取本地流失败, ${error}`);
});
},

/**
 * 摄像头开关
 */
toggleCamera() {
    this.enableCamera = !this.enableCamera
    this.ticSdk.enableCamera(this.enableCamera);
},

/**
 * 麦克风开关
 */
    toggleMic() {
        this.enableMic = !this.enableMic
        this.ticSdk.enableMic(this.enableMic);
    },

/**
 * 枚举摄像头
 */
    getCameraDevices() {
        this.ticSdk.getCameraDevices(devices => {
            this.devices.camera = devices;
    });
},

/**
 * 切换摄像头
 */
switchCamera() {
    if (this.cameraIndex < 0) {
        return;
    }
    this.ticSdk.switchCamera(this.devices.camera[this.cameraIndex]);
},

/**
 * 枚举麦克风
 */
    getMicDevices() {
        this.ticSdk.getMicDevices(devices => {
            this.devices.mic = devices;
    });
},

/**
 * 切换麦克风
 */
switchMic() {
    if (this.micIndex < 0) {
        return;
    }
    this.ticSdk.switchMic(this.devices.mic[this.micIndex]);
},

/**
 * 发送普通文本消息
 */
    sendMsg() {
        this.ticSdk.sendTextMessage(this.imMsg.common.data, this.imMsg.common.toUser);
        this.imMsg.common.data="";
    },

/**
 * 发送自定义消息
 */
    sendCustomGroupMsg() {
        this.ticSdk.sendCustomTextMessage({
            data: this.imMsg.custom.data,
            desc: this.imMsg.custom.desc,
            ext: this.imMsg.custom.ext
        }, this.imMsg.common.toUser);
    },

/**
举手
*/
    sendHandMsg(teacherUserName) {
        
        this.showTip("已举手，等待老师同意");
        this.ticSdk.sendCustomTextMessage({
            data: "6",
            desc: "test",
            ext: "apply_permission_notify"
        }, teacherUserName);
        
        this.ishand=false;
    },

triggerDialog: function (opt) {
    
    
    this.dialogTitle = opt.title;
    this.dialogContent = opt.content;
    this.dialogAction = opt.action;
    this.showDialog = true;
},
    dialogSubmit() {
        this.showDialog = false;
        this[this.dialogAction]();
    },
    dialogCancel() {
        this.dialogInfo=[];
        this.showDialog = false;
    },
acceptTeacher: function () {
    this.startRTC();
},
acceptStudent: function () { 
    this.ticSdk.sendCustomTextMessage({
        data: this.dialogInfo.permission,
        desc: "test",
        ext: "grant_permission_notify"
    }, this.dialogInfo.member);
},
permissionStudent: function (studentUserId,permission) { 
  
    //alert(Object.keys(this.remoteVideos).length);
    if(permission==6)
    {
        //老师同意举手
        for(var i=0;i<this.dialogInfo.length;i++){
            if( this.dialogInfo[i].member==studentUserId){
                this.dialogInfo.splice(i,1);
            }
        }
        if(this.dialogInfo.length==0){            
            this.showDialog = false;
        }
        if(Object.keys(this.remoteVideos).length==3){
            alert("已达到最大连接数");
            this.showDialog = false;
            return;
        }
    }

    this.ticSdk.sendCustomTextMessage({
        data: permission,
        desc: "test",
        ext: "grant_permission_notify"
    }, studentUserId);
    // this.dialogCancel();
},
permissionTeacher: function (studentUserId,permission) { 
    
    this.showTip('已点名'+studentUserId);
    this.ticSdk.sendCustomTextMessage({
        data: permission,
        desc: "test",
        ext: "invite_interact_notify"
    }, studentUserId);
},
teacherOpenHand: function () { 
    this.enableHand = !this.enableHand
    var permission='1';
    if(this.enableHand){
        permission='0';
        this.showTip("已开放学生举手");
    }else{
        this.showTip("已关闭学生举手");
    }
    
    this.dialogInfo=[];
    this.ticSdk.sendCustomTextMessage({
        data: permission,
        desc: "test",
        ext: "invite_interact_notify"
    }, "");
},
/**
 * 设置全局背景色
 * @param {*} color Hex 色值，如 #ff00ff
 */
    setGlobalColor(color) {
        this.ticSdk.getBoardInstance().setGlobalBackgroundColor(color);
    },

/**
 * 设置当前页背景色
 * @param {*} color Hex 色值，如 #ff00ff
 */
    setBgColor(color) {
        this.ticSdk.getBoardInstance().setBackgroundColor(color);
    },

/**
 * 设置涂鸦颜色
 * @param {*} color Hex 色值，如 #ff00ff
 */
    setColor(color) {
        this.ticSdk.getBoardInstance().setColor(color);
    },

/**
 * 设置涂鸦类型
 * @param {*} type 
 */
    setType(type) {
        this.ticSdk.getBoardInstance().setType(type);
    },

/**
 * 设置涂鸦粗细
 * @param {*} num 
 */
    setThin(num) {
        this.ticSdk.getBoardInstance().setThin(num);
        this.ticSdk.getBoardInstance().setRadius(num);
    },

/**
 * 清空当前页涂鸦(保留背景色/图片)
 */
    clearDraws() {
        this.ticSdk.getBoardInstance().clearDraws();
    },

/**
 * 清空当前页涂鸦 + 背景色/图片
 */
    clear() {
        this.ticSdk.getBoardInstance().clear();
    },

/**
 * 清除全局背景色
 */
    clearGlobal() {
        this.ticSdk.getBoardInstance().clearGlobalBgColor();
    },

/**
 * 回退
 */
    revert() {
        this.ticSdk.getBoardInstance().undo();
    },

/**
 * 恢复
 */
    process() {
        this.ticSdk.getBoardInstance().redo();
    },

/**
 * 上传文件
 */
    uploadFile() {
        var file = document.getElementById('file_input').files[0];
        if (file && /\.(bmp|jpg|jpeg|png|gif|webp|svg|psd|ai)$/i.test(file.name)) {
            this.showTip('图片正在上传，请等待');
            this.ticSdk.addImgFile(file, (total, data) => {
                this.showTip('图片上传成功');
            document.getElementById('file_input').value = '';
            document.getElementById('divPage').style.display='block';
            //document.getElementById('divOpenFile').style.display='none';
            //document.getElementById('divCloseFile').style.display='block';
        }, (err) => {
            console.error(err);
        this.showErrorTip('图片上传失败，请重试');
        document.getElementById('file_input').value = '';
    });
} else {
    this.showTip('文件正在上传，请等待');
    this.ticSdk.addFile(file, (total, data) => {
        this.showTip('文件上传成功，共' + total + '页');
    document.getElementById('file_input').value = '';
    document.getElementById('divPage').style.display='block';
    //document.getElementById('divOpenFile').style.display='none';
    //document.getElementById('divCloseFile').style.display='block';
}, (err) => {
    console.error(err);
this.showErrorTip('文件上传失败，请重试');
document.getElementById('file_input').value = '';
});
}
},


/**
 * 上一页
 */
prevBoard() {
    this.ticSdk.getBoardInstance().prevBoard();
},

/**
 * 下一页
 */
    nextBoard() {
        this.ticSdk.getBoardInstance().nextBoard();
    },

/**
 * 新增一页
 */
    addBoard() {
        this.ticSdk.getBoardInstance().addBoard();
    },

/**
 * 删除当前页
 */
    deleteBoard() {
        this.ticSdk.getBoardInstance().deleteBoard();
    },


/**
 * 删除指定页
 */
    deleteBoard2() {
        var boardId = board.getBoardList()[1];
        this.ticSdk.getBoardInstance().deleteBoard(boardId);
    },

/**
 * 白板事件回调处理
 * @param {*} data 
 */
    proBoardData(data) {
        this.boardData.data.current = data.current;
        this.boardData.data.list = data.list;
        this.currentFile = data.currentFile;

        this.boardFileGroup = board.getAllFileInfo();

        this.boardData.page = {
            current: this.boardData.data.list.indexOf(data.current) + 1,
            total: this.boardData.data.list.length
        }
    },

    showErrorTip(title, time) {
        this.$toasted.error(title, {
            position: "top-right",
            duration: time || 2000
        });
    },

    showTip(title, time) {
        this.$toasted.show(title, {
            position: "top-right",
            duration: time || 2000
        });
    },

/**
 * 退出课堂
 */
    quit() {
        this.ticSdk.quitClassroom();
        this.showTip(`正在退出课堂`);
       
    },

/**
 * 切换文件
 */
    switchFile(file) {
        board.switchFile(file.fid);
    },

/**
 * 删除文件
 */
    deleteFile(file) {
        board.deleteFile(file.fid);
    }


},

beforeDestroy() {
    this.quit();
}
});