Vue.use(Toasted);
window.app = new Vue({
  el: '#app_box',
  data() {
    return {
      step: 'first',
      pushModel: 1, // 1  自动推流 0 手动推流
      account: localStorage.getItem('IIC_USERID') || TEST_ACCOUNT.users[0]['userId'],
      userID: sessionStorage.getItem('IIC_USERNAME'),
      sdkAppId: TEST_ACCOUNT.sdkappid,
      userSig: '',
      nickName: sessionStorage.getItem('IIC_NICKNAME'),
      roomInfo: '',
      roomID: Math.floor(Math.random() * 1000000000),
      isTeacher: 1,
      enableCamera: true,
      enableMic: true,
      users: TEST_ACCOUNT.users,
      msgs: [],
      isPushing: 0, // 是否正在推流
      isPushCamera: 0, // 是否推摄像头流
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

      remoteVideos: {}
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

    initData() {


      this.msgs = [];

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
        userSig: this.findUserSig(this.account),
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
        color: '#ff0000'
      }

      this.enableCamera = true;
      this.enableMic = true;
    },

    init() {
      this.initData();
      this.ticSdk = null;
      this.ticSdk = new TICSDK();

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
        this.$nextTick(() => {
          if (document.getElementById(data.videoId)) {
            document.getElementById(data.videoId).srcObject = data.stream;
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
        this.showTip('WebRTC ERROR_NOTIFY');
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
        window.board = app.ticSdk.getBoardInstance();
        window.WebRTC = this.ticSdk.getWebRTCInstance();

        // 如果是主动推流
        if (this.pushModel === 1) {
          this.startRTC();
        }
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
            } else if (opType === webim.GROUP_TIP_TYPE.QUIT) { // 退群通知
              this.msgs.push({
                send: '群消息提示：',
                content: content.getOpUserId() + '退群了'
              });
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
            } else if (type === 'TIMCustomElem') {
              this.msgs.push({
                send: msgs.getFromAccount() + '：',
                content: `data: ${content.getData()} desc: ${content.getDesc()} ext: ${content.getExt()}`
              });
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
          } else if (type === 'TIMCustomElem') {
            this.msgs.push({
              send: msgs.getFromAccount() + '：',
              content: `data: ${content.getData()} desc: ${content.getDesc()} ext: ${content.getExt()}`
            });
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
        console.log('白板SDK鉴权不通过');
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
        if (WebRTC.global.localStream) {
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
        if (WebRTC.global.localStream) {
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
    },

    /**
     * 获取userSig
     */
    findUserSig() {
      var userSig = null;
      for (var i = 0, len = this.users.length; i < len; i++) {
        if (this.account === this.users[i].userId) {
          userSig = this.users[i].userToken;
          break;
        }
      }
      return userSig;
    }
  },

  beforeDestroy() {
    this.quit();
  }
});