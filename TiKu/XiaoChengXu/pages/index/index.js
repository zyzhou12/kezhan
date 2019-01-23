
//index.js
//获取应用实例
const app = getApp()

// 常量
const CONSTANT = require('../../constant/Constant');


Page({
  data: {

  },
  //事件处理函数
  bindViewTap: function() {
    wx.navigateTo({
      url: '../logs/logs'
    })
  },
  onLoad: function (options) {
    // 获取tic组件
    this.txTic = this.selectComponent('#tx_board');
    // 获取webrtc组件
    this.webrtcroom = this.selectComponent('#webrtcroom');


    var loginData={
      "sdkAppId": "1400127140",
      "identifier": "userid_947645165_5",
      "userSig":"eJxNjl1PgzAUhv8LtxptC*2GdwuSyGSQZQO3xaQpa8GKfFjKZDP*dyvZMq9O8jznvO-5ttbh6o7t901fa6qPrbAeLGDdjlhyUWuZS6EM7DuhJKeuMyEOhgRTfN5ibWs409RW-N9xx0s6KsOgAwBEEzPOUgytVIKyXI-ZELkEgIs8CNXJpjYcAYghsgG4Si2rvxchdvAU2Q4ilzpZGLzwt16w9IbyBKv*VMg5StP45nNThi91MzT6K5uR6rDz1jFPIMyC92XwNnt2o4pvkw9MkjAO5o9BHqmjwhEAOVv5Yni9nz7tCpYtpJ9aP794mVvT"
    }

var that=this
    var joinsucc = {
      apply: function (obj, ag) {
        console.log("joinsucc")
        console.log(obj)
        console.log(ag)
        that.txTic.initBoard(boardData);

        that.startRTC();
       
      }
    }
    var joinfail = {
      apply: function (obj, ag) {
        console.log("joinfail")
        console.log(obj)
        console.log(ag)
      }
    }

    var loginsucc={
      apply: function (obj, ag){
        console.log("loginsucc")
          console.log(ag)

        that.txTic.joinClassRoom("11450628", joinsucc, joinfail)
      }
    }
    var loginfail = {
      apply: function (obj, ag) {
        console.log("loginfail")
        console.log(obj)
        console.log(ag)
      }
    }



    this.txTic.login(loginData, loginsucc, loginfail)

    

    var boardData = { "orientation":"horizontal"}
  //  this.txTic.initBoard(boardData);
  },
  succ:function(msg){
   
      console.log(msg)
  },
  startRTC:function(){
    
    this.setData({
      muted: !this.data.muted,
      userID: "userid_947645165_5",
      userSig: "eJxNjl1PgzAUhv8LtxptC*2GdwuSyGSQZQO3xaQpa8GKfFjKZDP*dyvZMq9O8jznvO-5ttbh6o7t901fa6qPrbAeLGDdjlhyUWuZS6EM7DuhJKeuMyEOhgRTfN5ibWs409RW-N9xx0s6KsOgAwBEEzPOUgytVIKyXI-ZELkEgIs8CNXJpjYcAYghsgG4Si2rvxchdvAU2Q4ilzpZGLzwt16w9IbyBKv*VMg5StP45nNThi91MzT6K5uR6rDz1jFPIMyC92XwNnt2o4pvkw9MkjAO5o9BHqmjwhEAOVv5Yni9nz7tCpYtpJ9aP794mVvT",
      sdkAppID: "1400127140",
      roomID: "11450628",
      privateMapKey: "ZELkEgIs8CNXJpjYcAYghsgG4Si2rvxchdvAU2Q4ilzpZGLzwt16w9IbyBKv"
    }, () => {

      this.txTic.sendGroupTextMsg("uuu", this.succ, this.succ)
     
      this.webrtcroom.start();
    });
  },
  onIMEvent(e) {
    let code = e.detail.code;
    let tag = e.detail.tag;
    let data = e.detail.detail;
    switch (tag) {
      case CONSTANT.IM.KICKED:
        this.showErrorToast('账号在其他地方登录，被踢下线');
        break;
      case CONSTANT.IM.MSG_NOTIFY:
        break;
    }
  },
  onBoardEvent(e) {
    const {
      tag,
      data
    } = e.detail;
    switch (tag) {
      // 白板 SDK 鉴权不通过
      case CONSTANT.BOARD.VERIFY_SDK_ERROR:
        break;
    }
  },
  onRoomEvent(e) {
    const { tag, code } = e.detail;
    switch (tag) {
      case 'error':
        // 打开摄像头失败
        if (code === CONSTANT.ROOM.ERROR_OPEN_CAMERA) {

        }
        break;
    }
  }
})
