var AVProVideoWebGL = {
    isNumber: function (item) {
        return typeof(item) === "number" && !isNaN(item);
    },
    assert: function (equality, message) {
        if (!equality)
            console.log(message);
    },
    count: 0,
    videos: [],
    hasVideos__deps: ["count", "videos"],
    hasVideos: function (videoIndex) {
        if (videoIndex) {
            if (videoIndex == -1) {
                return false;
            }

            if (_videos) {
                if (_videos[videoIndex]) {
                    return true;
                }
            }
        } else {
            if (_videos) {
                if (_videos.length > 0) {
                    return true;
                }
            }
        }

        return false;
    },
    AVPPlayerInsertVideoElement__deps: ["count", "videos", "hasVideos"],
    AVPPlayerInsertVideoElement: function (path, idValues) {
        if (!path) {
            return false;
        }

        path = Pointer_stringify(path);
        _count++;

        var vid = document.createElement("video");
        var hasSetCanPlay = false;
        var playerIndex;
        var id = _count;
        var vidData = {
            id: id,
            video: vid,
            ready: false,
            hasMetadata: false,
            buffering: false,
            textureIsFlipped: false,
            frameCount: 0,
            fps: 0
        };

        _videos.push(vidData);
        playerIndex = (_videos.length > 0) ? _videos.length - 1 : 0;

        vid.oncanplay = function () {
            if (!hasSetCanPlay) {
                hasSetCanPlay = true;
                vidData.ready = true;
            }
        };

        vid.onloadedmetadata = function () {
            vidData.hasMetadata = true;
        };

        vid.oncanplaythrough = function () {
            vidData.buffering = false;
        };

        vid.onplaying = function () {
            // buffering
            this.buffering = false;

            // Frame rate and count
            var initialTime = new Date().getTime();

            vidData.checkFrames = setInterval(function () {
                var vid = vidData.video;
                var frameCount;

                if (!vid) {
                    clearInterval(vidData.checkFrames);
                    return;
                }

                if (vid.webkitDecodedFrameCount) {
                    frameCount = vid.webkitDecodedFrameCount;
                }

                if (vid.mozDecodedFrames) {
                    frameCount = vid.mozDecodedFrames;
                }

                if (!frameCount) {
                    var defaultFPS = 25;
                    vidData.fps = defaultFPS;
                    vidData.frameCount = vid.duration * defaultFPS;
                    console.log("Defaulted FPS to 25");

                    return;
                }

                var currentTime = new Date().getTime();
                var totalTime = (currentTime - initialTime) / 1000.0;
                var decodedFPS = frameCount / totalTime;
                var decodedFrames = frameCount;

                vidData.fps = decodedFPS;
                vidData.frameCount = decodedFrames;

            }, 1000);
        };

        vid.onwaiting = function () {
            vidData.buffering = true;

            if (vidData.checkFrames) {
                clearInterval(vidData.checkFrames);
            }
        };

        vid.onpause = function () {
            if (vidData.checkFrames) {
                clearInterval(vidData.checkFrames);
            }
        };

        vid.onended = function () {
            if (vidData.checkFrames) {
                clearInterval(vidData.checkFrames);
            }
        };

        /*vid.ontimeupdate = function() {
         //console.log("vid current time: ", this.currentTime);
         };*/

        vid.onerror = function (texture) {
            var err = "unknown error";

            switch (vid.error.code) {
                case 1:
                    err = "video loading aborted";
                    break;
                case 2:
                    err = "network loading error";
                    break;
                case 3:
                    err = "video decoding failed / corrupted data or unsupported codec";
                    break;
                case 4:
                    err = "video not supported";
                    break;
            }

            console.log("Error: " + err + " (errorcode=" + vid.error.code + ")", "color:red;");
        };

        vid.src = path;

		HEAP32[(idValues>>2)] = playerIndex;
		HEAP32[(idValues>>2)+1] = id;

		return true;
    },
    AVPPlayerFetchVideoTexture__deps: ["videos", "hasVideos"],
    AVPPlayerFetchVideoTexture: function (playerIndex, texture) {
        if (!_hasVideos(playerIndex)) {
            return;
        }

        GLctx.bindTexture(GLctx.TEXTURE_2D, GL.textures[texture]);
        //GLctx.pixelStorei(GLctx.UNPACK_FLIP_Y_WEBGL, true);
        GLctx.texParameteri(GLctx.TEXTURE_2D, GLctx.TEXTURE_WRAP_S, GLctx.CLAMP_TO_EDGE);
        GLctx.texParameteri(GLctx.TEXTURE_2D, GLctx.TEXTURE_WRAP_T, GLctx.CLAMP_TO_EDGE);
        GLctx.texParameteri(GLctx.TEXTURE_2D, GLctx.TEXTURE_MIN_FILTER, GLctx.LINEAR);
        GLctx.texParameteri(GLctx.TEXTURE_2D, GLctx.TEXTURE_MAG_FILTER, GLctx.LINEAR);
        GLctx.texImage2D(GLctx.TEXTURE_2D, 0, GLctx.RGB, GLctx.RGB, GLctx.UNSIGNED_BYTE, _videos[playerIndex].video);
    },
    AVPPlayerUpdatePlayerIndex__deps: ["videos", "hasVideos"],
    AVPPlayerUpdatePlayerIndex: function (id) {
        var result = -1;

        if (!_hasVideos()) {
            return result;
        }

        _videos.forEach(function (currentVid, index) {
            if (currentVid.id == id) {
                result = index;
            }
        });

        return result;
    },
    AVPPlayerHeight__deps: ["videos", "hasVideos"],
    AVPPlayerHeight: function (playerIndex) {
        if (!_hasVideos(playerIndex)) {
            return 0;
        }

        return _videos[playerIndex].video.videoHeight;
    },
    AVPPlayerWidth__deps: ["videos", "hasVideos"],
    AVPPlayerWidth: function (playerIndex) {
        if (!_hasVideos(playerIndex)) {
            return 0;
        }

        return _videos[playerIndex].video.videoWidth;
    },
    AVPPlayerReady__deps: ["videos", "hasVideos"],
    AVPPlayerReady: function (playerIndex) {
        if (!_hasVideos(playerIndex)) {
            return false;
        }

        if (_videos) {
            if (_videos.length > 0) {
                if (_videos[playerIndex]) {
                    return _videos[playerIndex].ready;
                }
            }
        } else {
            return false;
        }

        //return _videos[playerIndex].video.readyState >= _videos[playerIndex].video.HAVE_CURRENT_DATA;
    },
    AVPPlayerClose__deps: ["videos", "hasVideos"],
    AVPPlayerClose: function (playerIndex) {
        if (!_hasVideos(playerIndex)) {
            return;
        }

        clearInterval(_videos[playerIndex].checkFrames);
        _videos[playerIndex].video.src = "";
        _videos[playerIndex].video.load();
        _videos[playerIndex].video = null;
        _videos.splice(playerIndex, 1);
    },
    AVPPlayerSetLooping__deps: ["videos", "hasVideos"],
    AVPPlayerSetLooping: function (playerIndex, loop) {
        if (!_hasVideos(playerIndex)) {
            return;
        }

        _videos[playerIndex].video.loop = loop;
    },
    AVPPlayerIsLooping__deps: ["videos", "hasVideos"],
    AVPPlayerIsLooping: function (playerIndex) {
        if (!_hasVideos(playerIndex)) {
            return false;
        }

        return _videos[playerIndex].video.loop;
    },
    AVPPlayerHasMetadata__deps: ["videos", "hasVideos"],
    AVPPlayerHasMetadata: function (playerIndex) {
        if (!_hasVideos(playerIndex)) {
            return false;
        }

        return _videos[playerIndex].video.hasMetadata;
    },
    AVPPlayerIsPlaying__deps: ["videos", "hasVideos"],
    AVPPlayerIsPlaying: function (playerIndex) {
        if (!_hasVideos(playerIndex)) {
            return false;
        }

        var video = _videos[playerIndex].video;

        return !(video.paused || video.ended || video.seeking || video.readyState < video.HAVE_FUTURE_DATA);
    },
    AVPPlayerIsSeeking__deps: ["videos", "hasVideos"],
    AVPPlayerIsSeeking: function (playerIndex) {
        if (!_hasVideos(playerIndex)) {
            return false;
        }

        return _videos[playerIndex].video.seeking;
    },
    AVPPlayerIsPaused__deps: ["videos", "hasVideos"],
    AVPPlayerIsPaused: function (playerIndex) {
        if (!_hasVideos(playerIndex)) {
            return false;
        }

        return _videos[playerIndex].video.paused;
    },
    AVPPlayerIsFinished__deps: ["videos", "hasVideos"],
    AVPPlayerIsFinished: function (playerIndex) {
        if (!_hasVideos(playerIndex)) {
            return false;
        }

        return _videos[playerIndex].video.ended;
    },
    AVPPlayerIsBuffering__deps: ["videos", "hasVideos"],
    AVPPlayerIsBuffering: function (playerIndex) {
        if (!_hasVideos(playerIndex)) {
            return false;
        }

        return _videos[playerIndex].buffering;
    },
    AVPPlayerPlay__deps: ["videos", "hasVideos"],
    AVPPlayerPlay: function (playerIndex) {
        if (!_hasVideos(playerIndex)) {
            return;
        }

        _videos[playerIndex].video.play();
    },
    AVPPlayerPause__deps: ["videos", "hasVideos"],
    AVPPlayerPause: function (playerIndex) {
        if (!_hasVideos(playerIndex)) {
            return;
        }

        _videos[playerIndex].video.pause();
    },
    AVPPlayerStop__deps: ["videos", "hasVideos"],
    AVPPlayerStop: function (playerIndex) {
        if (!_hasVideos(playerIndex)) {
            return;
        }

        _videos[playerIndex].video.pause();
        _videos[playerIndex].video.currentTime = 0;
    },
    AVPPlayerRewind__deps: ["videos", "hasVideos", "AVPPlayerSeekToTime"],
    AVPPlayerRewind: function (playerIndex) {
        if (!_hasVideos(playerIndex)) {
            return;
        }

        _AVPPlayerSeekToTime(playerIndex, 0, true);
    },
    AVPPlayerSeekToTime__deps: ["videos", "hasVideos"],
    AVPPlayerSeekToTime: function (playerIndex, timeMs, fast) {
        if (!_hasVideos(playerIndex)) {
            return;
        }

        var vid = _videos[playerIndex].video;

        if (vid.seekable.length > 0) {
            for (var i = 0; i < vid.seekable.length; i++) {
                if(timeMs >= vid.seekable.start(i) && timeMs <= vid.seekable.end(i)) {
                    vid.currentTime = timeMs;
                    return;
                }
            }

            if(timeMs == 0) {
                _videos[playerIndex].video.load();
            }
        }
    },
    AVPPlayerSeekFast__deps: ["hasVideos"],
    AVPPlayerSeekFast: function (playerIndex, timeMs) {

    },
    AVPPlayerGetCurrentTime__deps: ["videos", "hasVideos"],
    AVPPlayerGetCurrentTime: function (playerIndex) {
        if (!_hasVideos(playerIndex)) {
            return 0;
        }

        return _videos[playerIndex].video.currentTime;
    },
    AVPPlayerGetVideoPlaybackRate__deps: ["videos", "hasVideos"],
    AVPPlayerGetVideoPlaybackRate: function (playerIndex) {
        if (!_hasVideos(playerIndex)) {
            return 0;
        }

        return _videos[playerIndex].video.fps;
    },
    AVPPlayerGetPlaybackRate__deps: ["videos", "hasVideos"],
    AVPPlayerGetPlaybackRate: function (playerIndex) {
        if (!_hasVideos(playerIndex)) {
            return 0;
        }

        return _videos[playerIndex].video.playbackRate;
    },
    AVPPlayerSetPlaybackRate__deps: ["videos", "hasVideos"],
    AVPPlayerSetPlaybackRate: function (playerIndex, rate) {
        if (!_hasVideos(playerIndex)) {
            return;
        }

        _videos[playerIndex].video.playbackRate = rate;
    },
    AVPPlayerSetMuted__deps: ["videos", "hasVideos"],
    AVPPlayerSetMuted: function (playerIndex, mute) {
        if (!_hasVideos(playerIndex)) {
            return;
        }

        _videos[playerIndex].video.muted = mute;
    },
    AVPPlayerGetDuration__deps: ["videos", "hasVideos"],
    AVPPlayerGetDuration: function (playerIndex) {
        if (!_hasVideos(playerIndex)) {
            return 0;
        }

        return _videos[playerIndex].video.duration;
    },
    AVPPlayerIsMuted__deps: ["videos", "hasVideos"],
    AVPPlayerIsMuted: function (playerIndex) {
        if (!_hasVideos(playerIndex)) {
            return false;
        }

        return _videos[playerIndex].video.muted;
    },
    AVPPlayerSetVolume__deps: ["videos", "hasVideos"],
    AVPPlayerSetVolume: function (playerIndex, volume) {
        if (!_hasVideos(playerIndex)) {
            return;
        }

        _videos[playerIndex].video.volume = volume;
    },
    AVPPlayerGetVolume__deps: ["videos", "hasVideos"],
    AVPPlayerGetVolume: function (playerIndex) {
        if (!_hasVideos(playerIndex)) {
            return 0;
        }

        return _videos[playerIndex].video.volume;
    },
    AVPPlayerHasVideo__deps: ["videos", "hasVideos"],
    AVPPlayerHasVideo: function (playerIndex) {
        if (!_hasVideos(playerIndex)) {
            return false;
        }

        return Boolean(_videos[playerIndex].video.webkitVideoDecodedByteCount) || Boolean(_videos[playerIndex].video.videoTracks && _videos[playerIndex].video.videoTracks.length);
    },
    AVPPlayerHasAudio__deps: ["videos", "hasVideos"],
    AVPPlayerHasAudio: function (playerIndex) {
        if (!_hasVideos(playerIndex)) {
            return false;
        }

        return _videos[playerIndex].video.mozHasAudio || Boolean(_videos[playerIndex].video.webkitAudioDecodedByteCount) || Boolean(_videos[playerIndex].video.audioTracks && _videos[playerIndex].video.audioTracks.length);
    },
    AVPPlayerGetFrameCount__deps: ["hasVideos"],
    AVPPlayerGetFrameCount: function (playerIndex) {
        if (!_hasVideos(playerIndex)) {
            return false;
        }

        var vid = _videos[playerIndex].video;

        if (vid.readyState <= HTMLMediaElement.HAVE_CURRENT_DATA || vid.paused) {
            return;
        }

        return _videos[playerIndex].frameCount;
    },
    AVPPlayerTextureIsFlipped__deps: ["videos", "hasVideos"],
    AVPPlayerTextureIsFlipped: function (playerIndex) {
        if (!_hasVideos(playerIndex)) {
            return false;
        }

        return !_videos[playerIndex].textureIsFlipped;
    },
    AVPPlayerGetFrameRate__deps: ["hasVideos"],
    AVPPlayerGetFrameRate: function (playerIndex) {
        if (!_hasVideos(playerIndex)) {
            return false;
        }

        var vid = _videos[playerIndex].video;
        if (vid.readyState <= HTMLMediaElement.HAVE_CURRENT_DATA || vid.paused) {
            return;
        }

        return _videos[playerIndex].fps;
    }
};

autoAddDeps(AVProVideoWebGL, 'count');
autoAddDeps(AVProVideoWebGL, 'videos');
autoAddDeps(AVProVideoWebGL, 'hasVideos');
mergeInto(LibraryManager.library, AVProVideoWebGL);