var CircularJSON = require('../libs/circular-json');

function ELKReport() {
  this.wxSysData = wx.getSystemInfoSync();
}

ELKReport.prototype = {
  APIURL: 'https://ilivelog.qcloud.com',

  initData(sdkappid, userId) {
    this.defDatas['sdkappid'] = sdkappid;
    this.defDatas['userid'] = userId;
  },

  setRoomId(roomID) {
    this.defDatas['roomid'] = roomID;
  },

  send: function (datas) {
    this.defDatas.brand = this.wxSysData.brand; // 手机品牌
    this.defDatas.model = this.wxSysData.model; // 手机型号
    this.defDatas.platform = this.wxSysData.platform; //手机平台
    this.defDatas.system = this.wxSysData.system; // 手机系统版本
    this.defDatas.weixinversion = this.wxSysData.version; //微信版本

    try {
      datas = this.fillDatas(datas);
      wx.request({
        url: this.APIURL,
        data: JSON.stringify(datas),
        method: "POST",
        header: {
          'content-type': 'application/x-www-form-urlencoded'
        },
        success: function (res) {

        },
        fail: function (res) {

        }
      });
    } catch (e) {
      console.log(e);
    }
  },

  fillDatas: function (datas) {
    var self = this;
    var dt = {};
    for (var p in self.defDatas) {
      dt[p] = typeof datas[p] != 'undefined' ? datas[p] : self.defDatas[p];
    }
    return dt;
  },

  defDatas: {
    brand: null, // 手机品牌
    model: null, // 手机型号
    platform: null, //手机平台
    system: null, // 手机系统版本
    weixinversion: null, //微信版本

    sdkappid: null,
    userid: null,
    roomid: null,

    device: "miniprog", // 事件对应的平台
    business: 'ticsdk', //  业务sdk名字
    action: null, // 	事件名称
    action_result: 0, // 事件错误码
    action_info: "", // 	错误详细信息
    extra_info: "", // 可扩展字段
    image_url: '', // 图片下载地址，设置白本背景图时需要带上
    color: null, // 颜色值，设置白板背景色时需要带上
    boardid: '', // 白板id, 所有在白板上的操作都要带上
  },

  reportData(action, data = {
    action_result: 0
  }) {
    if (data && data.extra_info) {
      data.extra_info = CircularJSON.stringify(data.extra_info)
    }
    this.send(Object.assign(data, {
      action
    }));
  }
};
module.exports = new ELKReport();