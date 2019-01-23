// 常量
const CONSTANT = require('../../constant/Constant');

const webim = require('../webim-component/webim_wx');

// webim组件
const webimComponent = require('../webim-component/webim-component');

const elkReport = require('../elk-component/ELKReport');

Component({

  /**
   * 组件的属性列表
   */
  properties: {

  },

  /**
   * 组件的初始数据
   */
  data: {
    txBoard: null,
    ticData: {},
    orientation: 'vertical'
  },

  /**
   * 组件的方法列表
   */
  methods: {
    /**
     * 登录im
     * @param {*} params tic所需参数
     * @param {*} succ  成功回调
     * @param {*} fail  失败回调
     */
    login(params, succ, fail) {
      // 初始化数据
      elkReport.initData(params.sdkAppId, params.identifier);

      this.setData({
        ticData: params
      }, () => {
        // 初始化数据
        webimComponent.initData({
          sdkAppID: params.sdkAppId,
          appIDAt3rd: params.sdkAppId, //用户所属应用id，必填
          identifier: params.identifier,
          userSig: params.userSig
        });
        // 初始化Im事件回调
        webimComponent.initLoginListeners(this.imLoginListener());
        // 登录IM
        webimComponent.loginIm(function () {
          elkReport.reportData('imlogin', {
            action_result: 0
          });
          succ.apply(this, arguments);
        }, function (error) {
          elkReport.reportData('imlogin', {
            action_result: error.ErrorCode,
            action_info: error.ErrorInfo
          });
          fail.apply(this, arguments);
        });
      });
    },

    logout(succ, fail) {
      if (webim.checkLogin()) {
        webim.logout(succ, fail);
      }
    },

    /**
     * 进入课堂
     * @param {*} roomID 课堂ID
     * @param {*} succ 进入成功的回调
     * @param {*} fail 进入失败的回调
     */
    joinClassRoom(roomID, succ, fail) {
      // 初始化数据
      roomID = roomID * 1;
      elkReport.setRoomId(roomID);
      console.log('joinClassRoom', roomID);
      this.data.ticData['roomID'] = roomID;
      this.setData({
        ticData: this.data.ticData
      }, () => {
        // 加入群组
        webimComponent.joinGroup(roomID + '', function () {
          elkReport.reportData('join_classroom', {
            action_result: 0
          });
          succ.apply(this, arguments);
        }, function (error) {
          elkReport.reportData('join_classroom', {
            action_result: error.ErrorCode,
            action_info: error.ErrorInfo
          });
          fail.apply(this, arguments);
        });
      });
    },

    /**
     * 退出课堂
     * @param {*} succ 退出成功
     * @param {*} fail 退出失败
     */
    quitClassRoom(succ, fail) {
      webimComponent.quitGroup(this.data.ticData['roomID'], succ, fail);
    },

    /**
     * 初始化白板
     */
    initBoard(borderConfig = {}) {
      var txBoard = this.data.txBoard = this.selectComponent('#tx_board_component');
      borderConfig = Object.assign({}, {
        identifier: this.data.ticData.identifier,
        userSig: this.data.ticData.userSig,
        sdkAppId: this.data.ticData.sdkAppId,
        roomID: this.data.ticData.roomID,
        orientation: this.data.orientation
      }, borderConfig);

      // 开始白板
      txBoard.render(borderConfig);
      // 上报初始化白板
      elkReport.reportData('ininBoard', {
        action_result: 0
      });
    },

    /**
     * 监听IM事件
     */
    imLoginListener() {
      var self = this;
      return {
        // 用于监听用户连接状态变化的函数，选填
        onConnNotify(resp) {
          self.fireIMEvent(CONSTANT.IM.CONNECTION_EVENT, resp.ErrorCode, resp);
        },

        // 监听新消息(直播聊天室)事件，直播场景下必填
        onBigGroupMsgNotify(msgs) {
          if (msgs.length) { // 如果有消息才处理
            self.fireIMEvent(CONSTANT.IM.BIG_GROUP_MSG_NOTIFY, 0, msgs);
          }
        },

        // 监听新消息函数，必填
        onMsgNotify(msgs) {
          if (msgs.length) { // 如果有消息才处理
            msgs.forEach(msg => {
              var elems = msg.elems;
              if (elems.length) {
                if (elems[0].type === 'TIMCustomElem' && elems[0].content.ext === 'TXConferenceExt') {
                  return;
                } else if (elems[0].type === 'TIMCustomElem' && elems[0].content.ext === 'TXWhiteBoardExt') {
                  // 如果是白板实时消息
                  msg.elems.forEach((elem) => {
                    self.data.txBoard.addBoardData(JSON.parse(elem.content.data));
                  })
                } else if (elems[0].type === 'TIMFileElem' && elems[0].content.name === 'TXWhiteBoardExt') {
                  // TODO 文件类型
                } else { // 普通消息外抛
                  self.fireIMEvent(CONSTANT.IM.MSG_NOTIFY, 0, msg);
                }
              }
            });
          }
        },

        // 系统消息
        onGroupSystemNotifys: {
          "1": (notify) => {
            self.fireIMEvent(CONSTANT.IM.GROUP_SYSTEM_NOTIFYS, {
              event_type: 1,
              notify
            });
          }, //申请加群请求（只有管理员会收到）
          "2": (notify) => {
            self.fireIMEvent(CONSTANT.IM.GROUP_SYSTEM_NOTIFYS, {
              event_type: 2,
              notify
            });
          }, //申请加群被同意（只有申请人能够收到）
          "3": (notify) => {
            self.fireIMEvent(CONSTANT.IM.GROUP_SYSTEM_NOTIFYS, {
              event_type: 3,
              notify
            });
          }, //申请加群被拒绝（只有申请人能够收到）
          "4": (notify) => {
            self.fireIMEvent(CONSTANT.IM.GROUP_SYSTEM_NOTIFYS, {
              event_type: 4,
              notify
            });
          }, //被管理员踢出群(只有被踢者接收到)
          "5": (notify) => {
            self.fireIMEvent(CONSTANT.IM.GROUP_SYSTEM_NOTIFYS, {
              event_type: 5,
              notify
            });
          }, //群被解散(全员接收)
          "6": (notify) => {
            self.fireIMEvent(CONSTANT.IM.GROUP_SYSTEM_NOTIFYS, {
              event_type: 6,
              notify
            });
          }, //创建群(创建者接收)
          "7": (notify) => {
            self.fireIMEvent(CONSTANT.IM.GROUP_SYSTEM_NOTIFYS, {
              event_type: 7,
              notify
            });
          }, //邀请加群(被邀请者接收)
          "8": (notify) => {
            self.fireIMEvent(CONSTANT.IM.GROUP_SYSTEM_NOTIFYS, {
              event_type: 8,
              notify
            });
          }, //主动退群(主动退出者接收)
          "9": (notify) => {
            self.fireIMEvent(CONSTANT.IM.GROUP_SYSTEM_NOTIFYS, {
              event_type: 9,
              notify
            });
          }, //设置管理员(被设置者接收)
          "10": (notify) => {
            self.fireIMEvent(CONSTANT.IM.GROUP_SYSTEM_NOTIFYS, {
              event_type: 10,
              notify
            });
          }, //取消管理员(被取消者接收)
          "101": (notify) => {
            self.fireIMEvent(CONSTANT.IM.GROUP_SYSTEM_NOTIFYS, {
              event_type: 101,
              notify
            });
          }, //群已被回收(全员接收)
          "102": (notify) => {
            self.fireIMEvent(CONSTANT.IM.GROUP_SYSTEM_NOTIFYS, {
              event_type: 102,
              notify
            });
          }, // 
          "201": (notify) => {
            self.fireIMEvent(CONSTANT.IM.GROUP_SYSTEM_NOTIFYS, {
              event_type: 201,
              notify
            });
          }, // 
          "301": (notify) => {
            self.fireIMEvent(CONSTANT.IM.GROUP_SYSTEM_NOTIFYS, {
              event_type: 301,
              notify
            });
          }, // 
          "255": (notify) => {
            self.fireIMEvent(CONSTANT.IM.GROUP_SYSTEM_NOTIFYS, {
              event_type: 255,
              notify
            });
          } //用户自定义通知(默认全员接收)
        },

        // 监听群资料变化事件，选填
        onGroupInfoChangeNotify(groupInfo) {
          self.fireIMEvent(CONSTANT.IM.GROUP_INFO_CHANGE_NOTIFY, 0, groupInfo);
        },

        // 被踢下线
        onKickedEventCall() {
          self.fireIMEvent(CONSTANT.IM.KICKED);
        }
      }
    },

    /**
     * 发送C2C文本消息
     * @param {*} receiveUser 
     * @param {*} msg 
     * @param {*} succ 
     * @param {*} fail 
     */
    sendC2CTextMsg(receiveUser, msg, succ, fail) {
      webimComponent.sendTextMessage(webim.SESSION_TYPE.C2C, receiveUser, msg, succ, fail);
    },

    /**
     * 发送C2C自定义消息
     * @param {object} msgObj {data: 'xxx', desc: 'xxxx', ext: 'xxxx'}
     * @param {function} succ
     * @param {function} fail
     */
    sendC2CCustomMsg(toUser, msgObj, succ, fail) {
      webimComponent.sendCustomMsg(webim.SESSION_TYPE.C2C, toUser, msgObj, succ, fail);
    },


    /**
     * 发送群组文本消息
     * @param {string} msg 
     * @param {function} succ 
     * @param {function} fail
     */
    sendGroupTextMsg(msg, succ, fail) {
      webimComponent.sendTextMessage(webim.SESSION_TYPE.GROUP, null, msg, succ, fail);
    },

    /**
     * 发送群组自定义消息
     * @param {object} msgObj {data: 'xxx', desc: 'xxxx', ext: 'xxxx'}
     * @param {function} succ
     * @param {function} fail
     */
    sendGroupCustomMsg(msgObj, succ, fail) {
      webimComponent.sendCustomMsg(webim.SESSION_TYPE.GROUP, null, msgObj, succ, fail);
    },

    /**
     * 设置白板显示方向
     * @param {*} orientation 
     */
    setOrientation(orientation) {
      elkReport.reportData('set_orientation', {
        extra_info: {
          orientation: orientation
        }
      });
      this.data.txBoard.setOrientation(orientation);
    },

    /**
     * 监听到白板事件
     * @param {*} e 
     */
    onBoardEvent(e) {
      this.fireBoardEvent(e.detail.tag, e.detail.data);
    },

    // 触发im事件
    fireIMEvent(tag, code, detail) {
      elkReport.reportData('im_event', {
        extra_info: {
          tag: tag,
          code: code,
          data: detail
        }
      });
      this.triggerEvent('IMEvent', {
        tag: tag,
        code: code,
        detail: detail
      });
    },

    // 触发白板事件
    fireBoardEvent(tag, data) {
      elkReport.reportData('border_event', {
        extra_info: {
          tag: tag,
          data: data
        }
      });
      this.triggerEvent('BoardEvent', {
        tag: tag,
        data: data
      });
    }
  }
})