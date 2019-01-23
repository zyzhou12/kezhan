// components/board-component/board-component.js

const BoardSDK = require('./libs/board_mini_prog_sdk.mini.js');
const Constant = require('../../constant/Constant');
const elkReport = require('../elk-component/ELKReport');

Component({
  /**
   * 组件的属性列表
   */
  properties: {
    identifier: {
      type: String,
      value: ''
    },

    userSig: {
      type: String,
      value: ''
    },

    sdkAppID: {
      type: Number,
      value: null
    },
    roomID: {
      type: Number,
      value: null
    },

    preStep: {
      type: Number,
      value: 2
    },

    // // 默认垂直方向
    // orientation: {
    //   type: String,
    //   value: 'vertical',
    //   observer: function (newValue) {
    //     this.setOrientation(newValue);
    //   }
    // }
  },

  /**
   * 组件的初始数据
   */
  data: {
    board: null,
    currentPic: "", //预留图片链接
    imgLoadList: null, //预加载图片链接列表
    bgColor: '#ffffff',
    naturalWidth: 0,
    naturalHeight: 0,
    imgStyle: '',
    boardWidth: 0,
    boardHeight: 0
  },

  ready() {

  },

  /**
   * 组件的方法列表
   */
  methods: {
    render(borderConfig) {
      wx.createSelectorQuery().in(this).select('.tic-board-box .tic_board_canvas').boundingClientRect((rect) => {
        this.setData({
          boardWidth: rect.width,
          boardHeight: rect.height
        }, () => {

          this.init(borderConfig, rect.width, rect.height);
        });
      }).exec();
    },

    init(borderConfig, width, height) {
      var canvasComponent = wx.createCanvasContext('tic_board_canvas', this);
      this.data.board = new BoardSDK({
        conf_id: borderConfig.roomID,
        canDraw: false,
        color: borderConfig.color || '#0f0000',
        preStep: this.data.preStep, //预加载步数
        tlsData: {
          identifier: borderConfig.identifier,
          userSig: borderConfig.userSig,
          sdkAppId: borderConfig.sdkAppId
        },
        width,
        height,
        canvasComponent,
        context: this,
        orientation: borderConfig.orientation
      });

      this.data.orientation = borderConfig.orientation;
      var width = this.data.boardWidth;
      var height = this.data.boardHeight;
      if (this.data.orientation === 'horizontal') { // 横屏
        height = this.data.boardWidth;
        width = this.data.boardHeight;
      } else {
        width = this.data.boardWidth;
        height = this.data.boardHeight;
      }
      this.setData({
        orientation: borderConfig.orientation,
        boardWidth: width,
        boardHeight: height
      });

      /*监听draw发送的预加载列表和图像链接 */
      this.data.board.on("preload", (data) => {
        console.log("preload");
        this.setData({
          currentPic: data.currentPic,
          imgLoadList: data.preloadList
        });
        console.log(data.preloadList);
      });

      this.data.board.on('add_board', (data) => {
        console.log('add_board');
        this.fireBoardEvent(Constant.BOARD.ADD_BOARD, data);
      });

      this.data.board.on('delete_board', (data) => {
        console.log('delete_board');
        this.fireBoardEvent(Constant.BOARD.DELETE_BOARD, data);
      });

      this.data.board.on('switch_board', (data) => {
        console.log('switch_board');
        this.fireBoardEvent(Constant.BOARD.SWITCH_BOARD, data);
      });

      // 增加文件
      this.data.board.on('add_group', gid => {
        this.fireBoardEvent(Constant.BOARD.ADD_GROUP, gid);
      });

      // 删除文件
      this.data.board.on('delete_group', gid => {
        this.fireBoardEvent(Constant.BOARD.DELETE_GROUP, gid);
      });

      // 切换文件
      this.data.board.on('switch_group', gid => {
        this.fireBoardEvent(Constant.BOARD.SWITCH_GROUP, gid);
      });

      // 白板SDK鉴权成功
      this.data.board.on('verify_sdk_succ', () => {
        this.fireBoardEvent(Constant.BOARD.VERIFY_SDK_SUCC);
      });

      // 白板SDK鉴权失败
      this.data.board.on('verify_sdk_error', () => {
        this.fireBoardEvent(Constant.BOARD.VERIFY_SDK_ERROR);
      });

      // 历史数据加载完成
      this.data.board.on('histroy_data_complete', () => {
        this.fireBoardEvent(Constant.BOARD.HISTROY_DATA_COMPLETE);
      });
    },

    /**
     * 图片加载完成
     * @param {*} ev 
     */
    imgOnLoad(ev) {
      let src = ev.currentTarget.dataset.src,
        width = ev.detail.width,
        height = ev.detail.height;

      // 获取图片原始宽高
      this.setData({
        naturalWidth: width,
        naturalHeight: height
      }, () => {
        this.updateImgStyle();
      });
      console.log('图片加载完成', ev);
    },

    // 图片加载失败
    imgOnLoadError(error) {
      console.log('图片加载失败', error);
    },

    // orientation
    setOrientation(orientation = 'vertical') {
      this.setData({
        orientation: orientation
      });
      wx.createSelectorQuery().in(this).select('.tic-board-box .tic_board_canvas').boundingClientRect((rect) => {
        var width = 0;
        var height = 0;
        if (this.data.orientation === 'horizontal') { // 横屏
          width = rect.height;
          height = rect.width;
        } else {
          width = rect.width;
          height = rect.height;
        }
        this.setData({
          boardWidth: width,
          boardHeight: height
        }, () => {
          if (this.data.board) {
            this.data.board.resize(width, height);
            this.data.board.setOrientation(orientation);
            this.updateImgStyle();
          }
        });
      }).exec();
    },

    addBoardData(data) {
      this.data.board.addData(data);
    },

    // 设置当前背景图片
    setCurrentImg(currentPic, currentBoard) {
      elkReport.reportData('load_img', {
        image_url: currentPic,
        boardid: currentBoard,
      });
      this.setData({
        currentPic
      });
    },

    // 设置预加载图片
    setPreLoadImgList(preloadList) {
      console.log(preloadList);
      elkReport.reportData('preload_img', {
        image_url: preloadList
      });
      this.setData({
        preloadList
      });
    },

    // 设置白板背景颜色
    setBoardBgColor(bgColor) {
      elkReport.reportData('set_bg_color', {
        color: bgColor
      });
      this.setData({
        bgColor
      });
    },

    // 更新图片样式
    updateImgStyle() {
      var style = [];
      if (this.data.orientation !== 'horizontal') { // 水平方向
        style = ['left: 50%', 'transform: translate(-50%, -50%) ', 'top: 50%'];
        if (this.data.boardWidth / this.data.boardHeight < this.data.naturalWidth / this.data.naturalHeight) {
          style.push('width:100%');
          style.push('height:' + (this.data.boardWidth / this.data.naturalWidth * this.data.naturalHeight) + 'px');
        } else {
          style.push('width:' + this.data.boardHeight / this.data.naturalHeight * this.data.naturalWidth + 'px');
          style.push('height:100%');
        }
      } else {
        if (this.data.boardWidth / this.data.boardHeight < this.data.naturalWidth / this.data.naturalHeight) {
          style.push('top: 0');
          style.push('width:' + this.data.boardWidth + 'px');
          var height = (this.data.boardWidth / this.data.naturalWidth * this.data.naturalHeight);
          style.push('height:' + height + 'px');
          style.push('left:' + (this.data.boardHeight + height) / 2 + 'px');
        } else {
          style.push('left: ' + this.data.boardHeight + 'px');
          var width = this.data.boardHeight / this.data.naturalHeight * this.data.naturalWidth;
          style.push('width:' + width + 'px');
          style.push('height:' + this.data.boardHeight + 'px');
          style.push('top:' + (this.data.boardWidth - width) / 2 + 'px');
        }
      }
      this.setData({
        imgStyle: style.join(';')
      });
    },

    // 触发白板事件
    fireBoardEvent(tag, data) {
      this.triggerEvent('BoardEvent', {
        tag: tag,
        data: data
      });
    }
  }
})