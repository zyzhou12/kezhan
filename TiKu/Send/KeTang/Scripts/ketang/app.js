webim.Log.setOn(0);

var app = new Vue({
	el: '#app',
	data: {
		ready: true,
		status: 'unlogin', // 当前状态
		//url: 'http://123.206.118.43/conf_svr_sdk/conference_server/public/api/conference?sdkappid=1400042982',
		url: 'https://sxb.qcloud.com/conf_svr_sdk/conference_server/public/api/conference?sdkappid=1400042982', // https://sxb.qcloud.com/web-edu/ 访问需要换成https地址
		boardUrl: 'https://yun.tim.qq.com/v4/ilvb_edu/whiteboard?sdkappid=1400042982&contenttype=json',
		roomList: [],
		createInfo: {
			conf_name: '',
			owner_nick: ''
		},
		id: '', // 登陆id
		openid: '',
		userSig: '',
		conf_id: 0,
		room_id: 0,
		group_id: 0,
		board_group_id: 0,
		conf_owner: 0,
		conf_name: '',
		role: 'student', // 默认学生
		msgList: [],
		message: '',
		selSess: {},
		boardselSess: {},
		conf_member: [],
		conf_info: {},
		paint: null,
		videoList: [],
		listeners: {},
		teacherVideo: {
			main: {
				videoId: '',
				stream: ''
			},
			support: {
				videoId: '',
				stream: ''
			}
		},
		tab: 'camera',
		isFirstSwitchBoard: false,
		myStream: {},
		showOptions: false,
		showNum: [
			false, false, false
		],
		studentStatus: {
			camera: false,
			mic: false
		},
		isStudentFirst: true,
		currentColor: 'bg-red',
		cos_bucket: '',
		cos_region: '',
		cos_sign: '',
		showDialog: false,
		dialogTitle: '',
		dialogContent: '',
		dialogAction: '',
		dialogInfo: {},
		paintCurrentPage: 0,
		paintPageList: [],
		edu_username: '',
		edu_password: ''
	},
	mounted: function () {
		this.init();
	},
	watch: {
		showOptions: function (value) {
			console.log(value);
			if (!value) {
				for (var i = 0; i < this.showNum.length; i++) {
					Vue.set(this.showNum, i, false)
				}
			}
		}
	},
	methods: {
		init: function () {
			// 可以自己注册，也可以用已注册用户直接登录
			this.edu_username = sessionStorage.getItem('edu_username');
			this.edu_password = sessionStorage.getItem('edu_password');
			if (this.edu_username && this.edu_password) {
				this.login(this.edu_username, this.edu_password);
			} else {
				this.register();
			}
		},
		login: function (id, pwd) {
			var self = this;
			self.sendRequest({
				"cmd": "open_account_svc",
				"sub_cmd": "verify",
				"password": pwd,
				"id": id
			}, function (res) {
				self.id = id;
				self.openid = id;
				self.userSig = res.user_sig;
				self.url += '&user_token=' + res.user_token + '&identifier=' + id;
				self.boardUrl += '&usersig=' + res.user_sig + '&identifier=' + id;
				// 这个时候得登陆IM
				self.loginIM(id, res.user_sig);
				self.getRoomList();
			})
		},
		loginIM: function (id, user_sig) {
			var self = this;
			webim.login({
				sdkAppID: 1400042982,
				identifier: id,
				identifierNick: id,
				userSig: user_sig,
				accountType: 17802
			}, {
				onConnNotify: function (res) {
					var info;
					switch (res.ErrorCode) {
						case webim.CONNECTION_STATUS.ON:
							webim.Log.warn('建立连接成功: ' + res.ErrorInfo);
					}
				},
				onMsgNotify: function (res) {
					// 解析每一个msg对象
					res.forEach(function (msg) {
						console.log(msg);
						var subType = msg.getSubType();
						var fromAccount = msg.fromAccount || '';
						var fromAccountNick = msg.fromAccountNick || fromAccount;

						if (msg.elems && msg.elems instanceof Array) { // 保证elems是数组
							var elems = msg.elems;
							elems.forEach(function (elem) {
								console.log(elem);
								if (elem.type == 'TIMTextElem') {
									// 普通消息
									if (fromAccountNick == 'admin' && elem.content.text == 'hello world')
										return;
									self.msgList.push({
										type: 'msg',
										from: fromAccountNick,
										content: elem.content.text
									});
								} else if (elem.type == 'TIMCustomElem') {
									var type = elem.content.ext;
									if (type == 'TXWhiteBoardExt') {
										if (msg.fromAccount == id)
											return;
										console.log(JSON.parse(elem.content.data));
										self.paint && self.paint.addData(JSON.parse(elem.content.data));
									}
								} else if (elem.type == 'TIMFileElem') {
									axios.get(elem.content.downUrl).then(function (response) {
										console.log(response.data);
										self.paint && self.paint.addData(response.data);
									})
								}
							});
						}
					});
				},
				onGroupSystemNotifys: {
					255: function (res) {
						var data = JSON.parse(res.UserDefinedField);
						console.log(data.sub_cmd);
						if (data.sub_cmd == 'member_join_notify') {
							var member = data.member;
							self.conf_member.push(member);
							self.msgList.push({
								type: 'admin',
								from: 'admin',
								content: member.nick + '加入了房间'
							});
						} else if (data.sub_cmd == 'member_quit_notify') {
							var member = data.member;
							self.msgList.push({
								type: 'admin',
								from: 'admin',
								content: self.getUserNick(member.id) + '离开了房间'
							});
							self.conf_member.forEach(function (item) {
								if (item.id == member.id) {
									item.status = 0;
									item.isOpen = false;
								}
							});
							self.videoList.forEach(function (item) {
								if (item.openId == member.id) {
									item.isOpen = false;
								}
							});
						} else if (data.sub_cmd == 'modify_member_info_notify') {
							console.log('----modify_member_info_notify');
							console.log(data.modify_infos);
							data.modify_infos.forEach(function (member) {
								if (member.status == 0) {
									self.msgList.push({
										type: 'admin',
										from: 'admin',
										content: self.getUserNick(member.id) + '离开了房间'
									});
									self.conf_member.forEach(function (item) {
										if (item.id == member.id) {
											item.status = 0;
											item.isOpen = false;
										}
									});
									self.videoList.forEach(function (item) {
										if (item.openId == member.id) {
											item.isOpen = false;
										}
									});
								}
								self.videoList.forEach(function (video, index) {
									if (member.id == video.openId) {
										console.log('-------是你', member.camera, video.isOpen);
										if ((member.hasOwnProperty('camera') && !member.camera && video.isOpen) || member.status == 0) {
											document.getElementById('video' + index) && (document.getElementById('video' + index).srcObject = null);
											video.isOpen = false;
										} else if (member.camera && !video.isOpen) {
											video.isOpen = true;
											Vue.nextTick(function () {
												self.listeners.onRemoteStreamAdd(video.stream, video.videoId, video.openId, 'video' + index);
											})
										}
									}
								})

								self.conf_member.forEach(function (item) {
									if (item.id == member.id) {
										console.log('修改');
										item = Object.assign(item, member);
									}
								});
							})
						} else if (data.sub_cmd == 'destroy_notify') {
							console.log('课程销毁');
							self.$toastr.error('课程被销毁，即将自动退出课堂', '课堂被销毁');
							setTimeout(function () {
								location.reload()
							}, 1000);
						} else if (data.sub_cmd == 'invite_interact_notify') {
							console.log('收到老师请求');
							self.triggerDialog({
								title: '是否接受老师连麦？',
								content: '收到老师连麦请求，是否接受？',
								action: 'acceptTeacher'
							})
						} else if (data.sub_cmd == 'apply_permission_notify') {
							console.log('收到学生请求');
							self.dialogInfo = {
								"conf_id": data.conf_id,
								"member": data.member,
								"permission": data.permission
							}
							self.triggerDialog({
								title: '是否接受学生' + self.getUserNick(data.member) + '连麦？',
								content: '收到学生' + self.getUserNick(data.member) + '连麦请求，是否接受？',
								action: 'acceptStudent'
							});
						} else if (data.sub_cmd == 'grant_permission_notify') {
							console.log('老师批准请求');
							console.log('------', data);
							if (data.permission == 0) {
								self.$toastr.success('被老师下麦', '提示');
								document.getElementById('me') && (document.getElementById('me').srcObject = null);
								WebRTCAPI.closeAudio();
								WebRTCAPI.closeVideo();
								self.videoList[0].isOpen = false;
								self.studentStatus.mic = false;
								self.studentStatus.camera = false;
								self.modifyInfo([{
									id: self.id,
									mic: 0,
									camera: 0
								}]);
								return;
							} else if (data.permission == 2) {
								self.$toastr.success('被老师关闭声音', '提示');
								WebRTCAPI.closeAudio();
								self.studentStatus.mic = false;
								self.modifyInfo([{
									id: self.id,
									mic: 0,
									camera: 1
								}]);
								return;
							} else if (data.permission >= 6 && self.studentStatus.camera == true) {
								self.$toastr.success('被老师打开声音', '提示');
								WebRTCAPI.openAudio();
								self.studentStatus.mic = true;
								self.modifyInfo([{
									id: self.id,
									mic: 1,
									camera: 1
								}]);
								return;
							} else if (data.permission >= 6) {
								self.$toastr.success('老师批准请求', '提示');
								self.videoList[0].isOpen = true;
								self.studentStatus.mic = true;
								self.studentStatus.camera = true;
								WebRTCAPI.openAudio();
								WebRTCAPI.openVideo();
								// 修改自己权限
								if (self.isStudentFirst) {
									WebRTCAPI.startWebRTC(function (result) {
										if (result !== 0) {
											var errorStr = "";
											if (result === -10007) {
												errorStr = "PeerConnection 创建失败";
											} else if (result === -10008) {
												errorStr = "getUserMedia 失败";
											} else if (result === -10009) {
												errorStr = "getLocalSdp 失败";
											} else {
												errorStr = "start WebRTC failed!!!";
											}
											console.error(errorStr, 'error');
											self.$toastr.error(errorStr, 'error');
										} else {
											self.modifyInfo([{
												id: self.id,
												mic: 1,
												camera: 1
											}])
										}
									});
									self.isStudentFirst = false;
								} else {
									self.modifyInfo([{
										id: self.id,
										mic: 1,
										camera: 1
									}]);
									Vue.nextTick(function () {
										var videoElement = document.getElementById("me");
										self.myStream && (videoElement.srcObject = self.myStream);
									})
								}
							}
						} else if (data.sub_cmd == 'modify_conf_info_notify') {
							console.log('修改课堂信息');
							var tempScreen = self.conf_info.home_screen;
							self.conf_info = Object.assign({}, self.conf_info, data.modify_infos);
							console.log(tempScreen, self.conf_info)
							if (self.role == 'student' && self.conf_info.home_screen != tempScreen && self.conf_info.home_screen.indexOf('whiteboard') == -1) {
								if (self.conf_info.home_screen.indexOf('camera') > -1) {
									self.listeners.onRemoteStreamAdd(self.teacherVideo.main.stream, self.teacherVideo.main.videoId, '', 'video');
								} else {
									self.listeners.onRemoteStreamAdd(self.teacherVideo.support.stream, self.teacherVideo.support.videoId, '', 'video');
								}
							}
						}
					}
				},
				onC2cEventNotifys: function (res) {}
			}, {});
		},
		modifyInfo: function (info) {
			var self = this;
			self.sendRequest({
				"cmd": "open_conf_svc",
				"sub_cmd": "modify_member_info",
				"conf_id": parseInt(self.conf_id, 10),
				"modify_infos": info
			}, function (res) {
				console.log('修改成功');
			})
		},
		register: function () {
			var self = this;
			var registerId = 'web_' + parseInt(Math.random() * 1000);
			var registerPwd = 'test';

			self.sendRequest({
				"cmd": "open_account_svc",
				"sub_cmd": "register",
				"id": registerId,
				"password": registerPwd
			}, function (res) {
				sessionStorage.setItem('edu_username', registerId);
				sessionStorage.setItem('edu_password', registerPwd);
				self.login(registerId, registerPwd);
			})
		},
		computePage: function (data) {
			this.paintCurrentPage = data.list.indexOf(data.current);
			this.paintPageList = data.list;
		},
		getRoomList: function (status) {
			var self = this;
			self.sendRequest({
				"cmd": "open_conf_svc",
				"sub_cmd": "get_conf_list"
			}, function (res) {
				console.log(res);
				if (status)
					self.$toastr.success('刷新列表', '提示');
				self.status = 'roomList';
				self.roomList = res.items;
			})
		},
		joinConf: function (id, nick_name) {
			var self = this;
			self.sendRequest({
				"cmd": "open_conf_svc",
				"sub_cmd": "join_conf",
				"conf_id": parseInt(id, 10),
				"nick": nick_name
			}, function (res) {
				self.group_id = res.chat_group_id;
				self.board_group_id = res.board_group_id;
				self.status = 'live';
				self.room_id = res.room_id;
				// 通知 加入会议
				self.sendRequest({
					"cmd": "open_conf_svc",
					"sub_cmd": "report_join_conf",
					"conf_id": parseInt(id, 10),
					"local_timestamp": self.getTimeStamp()
				}, function (ret) {
					if (ret.owner === self.edu_username) {
						self.role = 'teacher';
					}
					// 加入会议之后获取历史画板消息
					if (self.role == 'student') {
						// 学生可以直接初始化
						self.paint = new Sketch({
							id: 'sketch_pad',
							conf_id: self.conf_id,
							user: self.createInfo.owner_nick,
							tlsData: {
								sdkAppId: 1400042982,
								identifier: self.openid,
								userSig: self.userSig
							},
							canDraw: false,
							sendMsg: function (data) {
								console.log('sendMsg');
							},

							imgOnStartLoad: function (currentBoard, currentPicUrl, imgElSrc) {
								console.log('>>>>>>>> imgOnStartLoad', currentBoard, currentPicUrl, imgElSrc);
								self.$toastr.success('正在加载图片/PPT', '提示');
							},

							imgOnLoad: function (currentBoard, currentPicUrl, imgElSrc) {
								console.log('>>>>>>>> imgOnLoad', currentBoard, currentPicUrl, imgElSrc);
								self.$toastr.success('图片/PPT加载完成', '提示');
							},

							imgOnError: function (currentBoard, currentPicUrl, imgElSrc) {
								console.log('>>>>>>>> imgOnError', currentBoard, currentPicUrl, imgElSrc);
								self.$toastr.error('图片/PPT加载错误', '提示');
							},

							imgOnAbort: function (currentBoard, currentPicUrl, imgElSrc) {
								console.log('>>>>>>>> imgOnAbort', currentBoard, currentPicUrl, imgElSrc);
								self.$toastr.error('图片/PPT中断加载', '提示');
							}
						});
						// 同步白板数据
						self.paint.getBoardData();
					} else {
						self.tab = ret.home_screen || 'camera';
						if (self.tab.indexOf('whiteboard') > -1) {
							self.changeConf({
								home_screen: this.tab
							});
							if (!self.isFirstSwitchBoard) {
								console.log('第一次切到白板，初始化白板');
								self.isFirstSwitchBoard = true;
								Vue.nextTick(function () {
									self.teacherInit();
								});
							}
						}
					}
					self.initWebRTC();
				});

				// 发送心跳
				self.heartBeat(id, res.heartbeat_interval || 10);
				self.selSess = new webim.Session(webim.SESSION_TYPE.GROUP, self.group_id, self.group_id);
				self.boardselSess = new webim.Session(webim.SESSION_TYPE.GROUP, self.board_group_id, self.board_group_id);
				self.getCosInfo();
				// 获取房间成员
				self.getConfMember();
				// 初始化webrtc

			})
		},

		// 加入会议后初始化webrtc
		initWebRTC: function () {
			var self = this;
			self.listeners = {
				onMediaChange: function (ret) {
					console.log(ret)
				},
				onRemoteLeave: function (ret) {
					console.log(ret)
				},
				onIceConnectionClose: function (ret) {
					console.log(ret)
				},
				onChangeRemoteStreamState: function (ret) {
					console.log(ret)
				},
				onInitResult: function (ret) {
					console.log(ret)
					WebRTCAPI.createRoom({
						roomid: self.room_id,
						role: self.role == 'student' ?
							'Guest' : 'LiveMaster'
					}, function (result) {
						if (result !== 0) {
							console.error("create room failed!!!");
						} else {
							console.log('进入房间');
							if (self.role == 'teacher') {
								setTimeout(function () {
									self.modifyInfo([{
										id: self.id,
										mic: 1,
										camera: 1
									}]);
								}, 1000)
							}
						}
					});
				},
				onLocalStreamAdd: function (stream) {
					var videoElement = document.getElementById("me");
					videoElement.srcObject = stream;
					if (self.role == 'teacher') {
						var videoElement = document.getElementById("video");
						videoElement.srcObject = stream;
					} else {
						self.myStream = stream;
					}
				},
				onRemoteStreamAdd: function (stream, videoId, openId, id) {
					if (!stream)
						return;
					var videoElement = document.getElementById(id);
					console.log(videoElement);
					console.log(stream, videoId, openId, id);
					videoElement.srcObject = stream;
				},
				onRemoteStreamRemove: function (videoId) {
					console.log('---remove');
					if (videoId == self.teacherVideo.support.videoId) {
						self.listeners.onRemoteStreamAdd(self.teacherVideo.main.stream, self.teacherVideo.main.videoId, '', 'video');
					}
				},
				onRemoteStreamUpdate: function (data) {
					if (data.openId == self.conf_owner) {
						console.log('----老师');
						console.log(data.videoType);
						if (data.videoType == 7) {
							self.teacherVideo.support.stream = data.stream;
							self.teacherVideo.support.videoId = data.videoId;
						} else if (data.videoType == 2) {
							self.teacherVideo.main.stream = data.stream;
							self.teacherVideo.main.videoId = data.videoId;
						}
						self.listeners.onRemoteStreamAdd(data.stream, data.videoId, data.openId, 'video');
					} else {
						console.log('----学生');
						// 便利下有没有
						var isExist = false;
						self.videoList.forEach(function (item) {
							if (item.openId == data.openId) {
								isExist = true;
								// 更新一下
								item.stream = data.stream;
								item.videoId = data.videoId;
							}
						});

						if (!isExist) {
							self.videoList.push({
								isOpen: false,
								stream: data.stream,
								videoId: data.videoId,
								openId: data.openId
							})
						}
					}
				},
				onKickout: function (ret) {
					location.reload();
				},
				onWebSocketClose: function (ret) {
					console.log(ret)
				},
				onRelayTimeout: function (ret) {
					console.log(ret)
				}
			};
			WebRTCAPI.init(self.listeners, {
				sdkAppId: 1400042982,
				openid: self.openid,
				userSig: self.userSig,
				accountType: 17802,
				audio: true,
				video: true,
				useCloud: true,
				closeLocalMedia: self.role == 'student' ?
					true : false
			});

			self.videoList.push({
				id: self.id,
				isMe: true,
				isOpen: self.role == 'student' ?
					false : true
			}); // 把自己的信息存到第一个
		},

		applyPermission: function () {
			var self = this;
			if (self.studentStatus.mic || self.studentStatus.camera) {
				self.$toastr.error('已在连麦中', '提示');
				return;
			}
			self.sendRequest({
				"cmd": "open_conf_svc",
				"sub_cmd": "apply_permission",
				"conf_id": parseInt(self.conf_id, 10),
				"permission": 6
			}, function (ret) {
				self.$toastr.success('已申请权限', '提示');
			});
		},
		getConfMember: function () {
			var self = this;
			self.sendRequest({
				"cmd": "open_conf_svc",
				"sub_cmd": "get_member_list",
				"conf_id": parseInt(self.conf_id, 10)
			}, function (ret) {
				console.log(ret);
				// 记录了除去自己以外的全部的人
				self.conf_member = ret.member_list;
			});
		},
		getUserNick: function (id) {
			if (id == this.id) {
				return this.createInfo.owner_nick; // 说明是自己
			} else {
				for (var i = 0; i < this.conf_member.length; i++) {
					var item = this.conf_member[i];
					if (item.id == id) {
						return item.nick;
					}
				}
			}
			return id;
		},
		sendMsg: function (content) {
			// 发送普通消息
			var self = this;
			if (!self.message)
				return self.$toastr.error(error, '请输入发送内容');
			var message = new webim.Msg(self.selSess, true, -1, Math.round(Math.random() * 4294967296), Math.round(new Date().getTime() / 1000), this.id, webim.GROUP_MSG_SUB_TYPE.COMMON, this.createInfo.nick_name);
			var text = new webim.Msg.Elem.Text(self.message);
			message.addText(text);
			webim.sendMsg(message, function (resp) {
				console.log(resp);
				self.message = '';
			});
		},
		heartBeat: function (id, interval) {
			var self = this;
			var data = {
				"cmd": "open_conf_svc",
				"sub_cmd": "heart_beat",
				"conf_id": parseInt(id, 10)
			};
			setInterval(function () {

				self.sendRequest(data, function (res) {
					// 心跳成功
					console.log('heartBeat: data:', data, " ret:", JSON.stringify(res));
				});
			}, interval * 1000);
		},
		getCosInfo: function () {
			var self = this;
			self.sendRequest({
				"cmd": "open_cos_svc",
				"sub_cmd": "get_cos_sign",
				"type": 1
			}, function (ret) {
				self.cos_bucket = ret.bucket;
				self.cos_region = ret.region;
				self.cos_sign = ret.sign;
			});
		},
		setNick: function (id, conf_name, owner, conf) {
			this.conf_id = id;
			this.conf_name = conf_name;
			this.conf_owner = owner;
			this.conf_info = conf;
			this.status = 'joinRoom'
		},
		createConf: function () {
			var self = this;
			self.sendRequest({
				"cmd": "open_conf_svc",
				"sub_cmd": "create_conf",
				"conf_name": self.createInfo.conf_name,
				"home_screen": "camera#web"
			}, function (res) {
				// 加入课堂
				self.conf_info = res;
				self.conf_name = self.createInfo.conf_name;
				self.role = 'teacher';
				self.conf_id = res.conf_id;
				self.conf_owner = self.id;
				self.joinConf(res.conf_id, self.createInfo.owner_nick);
			})
		},
		changeConf: function (info, cb) {
			var self = this;
			self.sendRequest({
				"cmd": "open_conf_svc",
				"sub_cmd": "modify_conf_info",
				"conf_id": self.conf_id,
				"modify_infos": info
			}, function (res) {
				if (cb)
					cb();
			})
		},

		concat: function () {
			this.$modal.show('hello-world');
		},

		refresh: function () {
			var self = this;
			self.sendRequest({
				"cmd": "open_conf_svc",
				"sub_cmd": self.role == 'student' ?
					'quit_conf' : 'destroy_conf',
				"conf_id": parseInt(self.conf_id, 10),
				"reason": "课程结束"
			}, function (ret) {
				location.reload();
			});
		},
		forbid: function (id) {
			var self = this;
			self.sendRequest({
				"cmd": "open_conf_svc",
				"sub_cmd": "grant_permission",
				"conf_id": parseInt(self.conf_id, 10),
				"member": id,
				"permission": 0
			}, function (res) {
				console.log('修改成功');
			});
		},
		invite: function (id) {
			var self = this;
			self.sendRequest({
				"cmd": "open_conf_svc",
				"sub_cmd": "invite_interact",
				"conf_id": parseInt(self.conf_id, 10),
				"member": id,
				"permission": 6
			}, function (ret) {
				self.$toastr.success('已邀请', '提示');
			});
		},
		switchTab: function (tab) {
			var self = this;
			if (this.conf_info.home_screen != tab) {
				this.tab = tab;
				this.changeConf({
					home_screen: tab
				});
				if (!self.isFirstSwitchBoard && tab == 'whiteboard') {
					console.log('第一次切到白板，初始化白板');
					self.isFirstSwitchBoard = true;
					Vue.nextTick(function () {
						self.teacherInit();
					})
				}
			}
		},

		// 老师
		teacherInit: function () {
			var self = this;
			self.paint = new Sketch({
				id: 'sketch_pad',
				conf_id: self.conf_id,
				user: self.createInfo.owner_nick,
				canDraw: true,
				color: 4278255871,
				tlsData: {
					sdkAppId: 1400042982,
					identifier: self.openid,
					userSig: self.userSig
				},
				sendMsg: function (data) {
					self.sendPaintMsg(JSON.stringify(data));
				},
				infoAddBoard: function (data) {
					self.computePage(data);
				},
				infoSwitchBoard: function (data) {
					self.computePage(data);
				},
				infoDeleteBoard: function (data) {
					self.computePage(data);
				},

				imgOnStartLoad: function (currentBoard, currentPicUrl, imgElSrc) {
					console.log('>>>>>>>> imgOnStartLoad', currentBoard, currentPicUrl, imgElSrc);
					self.$toastr.success('正在加载图片/PPT', '提示');
				},

				imgOnLoad: function (currentBoard, currentPicUrl, imgElSrc) {
					console.log('>>>>>>>> imgOnLoad', currentBoard, currentPicUrl, imgElSrc);
					self.$toastr.success('图片/PPT加载完成', '提示');
				},

				imgOnError: function (currentBoard, currentPicUrl, imgElSrc) {
					console.log('>>>>>>>> imgOnError', currentBoard, currentPicUrl, imgElSrc);
					self.$toastr.error('图片/PPT加载错误', '提示');
				},

				imgOnAbort: function (currentBoard, currentPicUrl, imgElSrc) {
					console.log('>>>>>>>> imgOnAbort', currentBoard, currentPicUrl, imgElSrc);
					self.$toastr.error('图片/PPT中断加载', '提示');
				},

				event: [{
					type: 'mousedown',
					fn: function (event) {
						self.showOptions = false;
					}
				}]
			});
			// 同步白板数据
			self.paint.getBoardData();
		},

		sendPaintMsg: function (content) {
			var self = this;
			// if (content.length > 7000) {
			// var opt = {
			// 	'toAccount': self.board_group_id, 接收者
			// 	'businessType': webim.UPLOAD_PIC_BUSSINESS_TYPE.GROUP_MSG, 业务类型
			// 	'File_Type': webim.UPLOAD_RES_TYPE.FILE, 表示上传文件
			// 	'base64Str': btoa(content),
			// 	'totalSize': btoa(content).length,
			// 	'fileMd5': md5(btoa(content))
			// };
			// webim.uploadPicByBase64(opt, function(resp) {
			// 	console.log(resp)
			// }, function(err) {
			// 	alert(err.ErrorInfo);
			// });
			// } else {
			var msg = new webim.Msg(self.boardselSess, true, -1, Math.round(Math.random() * 4294967296), Math.round(new Date().getTime() / 1000), self.id, webim.GROUP_MSG_SUB_TYPE.COMMON, self.createInfo.nick_name);
			var custom = new webim.Msg.Elem.Custom(content, '', 'TXWhiteBoardExt');
			msg.addCustom(custom);
			msg.PushInfoBoolean = true;
			msg.PushInfo = {
				Ext: 'TXWhiteBoardExt',
				PushFlag: 0
			};
			webim.sendMsg(msg, function (resp) {
				console.log(resp);
			})
			// }
		},

		showOption: function (num) {
			this.showOptions = true;
			for (var i = 0; i < this.showNum.length; i++) {
				if (i == num) {
					Vue.set(
						this.showNum, i, this.showNum[i] ?
						false :
						true);
				} else {
					Vue.set(this.showNum, i, false)
				}
			}
			if (num == 1) {
				this.paint.setType('line');
			}
		},
		setPaintThin: function (num) {
			this.paint.setThin(num);
			this.paint.setRadius(num);
			this.showOptions = false;
		},
		setPaintColor: function (color, num) {
			this.paint.setColor(color);
			var list = [
				'bg-blue',
				'bg-green',
				'bg-yellow',
				'bg-red',
				'bg-black',
				'bg-gray'
			];
			this.currentColor = list[num];
			this.showOptions = false;
		},
		setPaintType: function (type) {
			this.paint.setType(type);
			this.showOptions = false;
		},
		// triggerUploadPic: function() {
		// 	document.getElementById("imageSelector").click();
		// },
		triggerUpload: function () {
			document.getElementById("fileSelector").click();
		},
		// uploadPic: function() {
		// 	var self = this;
		// 	this.uploadFile('imageSelector', function(result) {
		// 		self.paint.setBackgroundPic(result);
		// 	})
		// },
		upload: function () {
			var self = this;
			this.uploadFile('fileSelector', function (result, key) {
				var previewUrl = 'https://eddieli-1253488539.preview.myqcloud.com' + '/' + key + "?cmd=doc_preview&page=";
				// 如是图片类型
				if (/\.(bmp|jpg|jpeg|png|gif|webp)/i.test(previewUrl)) {
					self.paint.setBackgroundPic(result);
					return;
				}
				// 获取页数
				console.log(previewUrl);

				// self.sendRequest({
				// 	"cmd": "open_conf_svc",
				// 	"sub_cmd": "cos_user_returncode",
				// 	"conf_id": parseInt(self.conf_id, 10),
				// 	"cos_url": previewUrl + '0'
				// }, function (res) {
				// 	var page = res.user_returncode;
				// 	// 根据页数发消息
				// 	var time = +new Date();
				// 	for (var i = 0; i < page; i++) {
				// 		(function (index) {
				// 			setTimeout(function () {
				// 				self.paint.addBackgroundPic(previewUrl + (index + 1), true);
				// 			}, index * 100);
				// 		})(i);
				// 	}
				// }, true);

				axios.get(previewUrl + '1').then((res) => {
					if (res.status === 200 && res.headers['user-returncode']) {
						var totalPage = res.headers['user-returncode'] * 1;
						// 根据页数发消息
						var time = +new Date();
						for (var i = 0; i < totalPage; i++) {
							(function (index) {
								setTimeout(function () {
									self.paint.addBackgroundPic(previewUrl + (index + 1), true);
								}, index * 100);
							})(i);
						}
					} else {
						self.$toastr.error('获取文档总页数失败，请重试', '提示');
					}
				}, () => {
					self.$toastr.error('获取文档总页数失败，请重试', '提示');
				});
			})
		},
		uploadFile: function (id, cb) {
			var self = this;
			var file = document.getElementById(id).files[0];
			var index = file.name.lastIndexOf('.');
			var Key = self.id + '-' + (+new Date()) + file.name.substring(index);
			var fd = new FormData();
			fd.append('key', Key);
			fd.append('Signature', self.cos_sign);
			fd.append('success_action_status', '200');
			fd.append('file', file);

			var url = 'https://eddieli-1253488539.cos.ap-shanghai.myqcloud.com';
			var xhr = new XMLHttpRequest();
			xhr.open('POST', url, true);
			xhr.onload = function () {
				if (xhr.status === 200 || xhr.status === 206) {
					self.$toastr.success('上传成功', '提示');
					document.getElementById(id).value = '';
					var result = url + '/' + Key;
					cb(result, Key);
				} else {
					self.$toastr.error('上传失败', '提示');
				}
			};
			xhr.onerror = function () {
				alert('文件 ' + Key + ' 上传失败，请检查是否没配置 CORS 跨域规则');
			};
			xhr.send(fd);
			self.$toastr.success('开始上传', '提示');
		},
		triggerDialog: function (opt) {
			this.dialogTitle = opt.title;
			this.dialogContent = opt.content;
			this.dialogAction = opt.action;
			this.showDialog = true;
		},
		dialogSubmit: function () {
			this.showDialog = false;
			this[this.dialogAction]();
		},
		dialogCancel: function () {
			this.showDialog = false;
		},
		acceptTeacher: function () {
			var self = this;
			self.$toastr.success('接受老师请求,开始连麦', '提示');
			// 开启webrtc
			self.videoList[0].isOpen = true;
			self.studentStatus.mic = true;
			self.studentStatus.camera = true;
			WebRTCAPI.openAudio();
			WebRTCAPI.openVideo();
			if (self.isStudentFirst) {
				WebRTCAPI.startWebRTC(function (result) {
					if (result !== 0) {
						var errorStr = "";
						if (result === -10007) {
							errorStr = "PeerConnection 创建失败";
						} else if (result === -10008) {
							errorStr = "getUserMedia 失败";
						} else if (result === -10009) {
							errorStr = "getLocalSdp 失败";
						} else {
							errorStr = "start WebRTC failed!!!";
						}
						console.error(errorStr, 'error');
						self.$toastr.error(errorStr, 'error');
					} else {
						self.modifyInfo([{
							id: self.id,
							mic: 1,
							camera: 1
						}]);
						self.isStudentFirst = false;
					}
				});
			} else {
				self.modifyInfo([{
					id: self.id,
					mic: 1,
					camera: 1
				}]);
				Vue.nextTick(function () {
					var videoElement = document.getElementById("me");
					self.myStream && (videoElement.srcObject = self.myStream);
				})
			}
		},
		acceptStudent: function () { // 要传参
			var self = this;
			self.sendRequest({
				"cmd": "open_conf_svc",
				"sub_cmd": "grant_permission",
				"conf_id": self.dialogInfo.conf_id,
				"member": self.dialogInfo.member,
				"permission": self.dialogInfo.permission
			}, function (res) {
				console.log('修改成功');
			});
		},
		paintPrev: function () {
			if (this.paintCurrentPage == 0)
				return;
			var boardId = this.paintPageList[this.paintCurrentPage - 1];
			this.paint.switchBoard(boardId);
		},
		paintNext: function () {
			if (this.paintCurrentPage == (this.paintPageList.length - 1))
				return;
			var boardId = this.paintPageList[this.paintCurrentPage + 1];
			this.paint.switchBoard(boardId);
		},
		paintAdd: function () {
			this.paint.addBoard();
		},
		paintDelete: function () {
			this.paint.deleteBoard(this.paintPageList[this.paintCurrentPage]);
		},
		revert: function () {
			var self = this;
			if (self.paint.canRevert()) {
				self.paint.revert();
			} else {
				self.$toastr.error('无法继续回退', '错误');
			}
		},
		process: function () {
			var self = this;
			if (self.paint.canProcess()) {
				self.paint.process();
			} else {
				self.$toastr.error('已是最新操作,无法继续前进', '错误');
			}
		},
		getTimeStamp: function () {
			var time = +new Date();
			return parseInt(time & 0xFFFFFFFF, 10);
		},
		sendRequest: function (data, cb, special) {
			var self = this;
			axios.post(
				special ?
				this.boardUrl :
				this.url,
				data).then(function (response) {
				if (data.sub_cmd != 'register') {
					cb(response.data);
				} else {
					cb();
				}
			}).catch(function (error) {
				console.log(error);
				self.$toastr.error(error, '错误信息');
			});
		}
	}
});