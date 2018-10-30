/// <reference path="typings/jquery/jquery.d.ts" />
var DanSongAlbum = (function () {
    function DanSongAlbum(selector) {
        this.selector = selector;
        this.$album = $(selector);
        if (this.$album.length == 0)
            throw "No album found.";
        if (this.$album.length > 1)
            throw "Multiple albums found.";
        var $songs = this.$album.find(".danSong");
        if ($songs.length == 0)
            throw "No songs found.";
        this.curSongIdx = 0;
        this.songs = $.map($songs, function (item) {
            var $t = $(item);
            var $btn = $t.find(".danPlayPauseButton").first();
            var sng = {
                $danSong: $t,
                $title: $t.find(".danTitle").first(),
                $button: $btn,
                $btnGlyph: $btn.find("b"),
                trackNum: parseInt($btn.data("tracknum")),
                fileName: $btn.data("fname"),
                secsDur: parseInt($btn.data("secsdur")),
                $prog: $t.find(".danProgress").first(),
                $bar: $t.find(".danProgressBar").first()
            };
            return sng;
        });
    }
    //private ButtonIsCurrent(button: JQuery): boolean {
    //    if (!this.getCurSong) return false;
    //    let inFN = button.data("fname")
    //    return this.getCurSong.fileName == inFN;
    //}
    /**
     * Always a current song.
     */
    DanSongAlbum.prototype.getCurSong = function () {
        var cs = this.songs[this.curSongIdx];
        return cs;
    };
    DanSongAlbum.prototype.setCurSong = function (idx) {
        this.curSongIdx = idx;
        var cs = this.songs[idx];
        return cs;
    };
    DanSongAlbum.prototype.gotoPrevSong = function () {
        if (--this.curSongIdx < 0)
            this.curSongIdx = this.songs.length - 1;
        var cs = this.songs[this.curSongIdx];
        return cs;
    };
    DanSongAlbum.prototype.gotoNextSong = function () {
        if (++this.curSongIdx > this.songs.length - 1)
            this.curSongIdx = 0;
        var cs = this.songs[this.curSongIdx];
        return cs;
    };
    return DanSongAlbum;
}());
var DanAudioMgr = (function () {
    function DanAudioMgr(audioPlayerID, danSongAlbumSelector, eventCallback) {
        this.audioPlayerID = audioPlayerID;
        this.danSongAlbumSelector = danSongAlbumSelector;
        this.eventCallback = eventCallback;
        //debugger;
        this.album = new DanSongAlbum(danSongAlbumSelector);
        this.player = document.getElementById(audioPlayerID);
        var self = this;
        self.player.src = "/media/songs/" + self.album.getCurSong().fileName; // prime the pump!
        // browser can start playing
        // oncanplay ----------------------------
        // media has been started or is no longer paused
        this.player.onplay = function () {
            var sng = self.album.getCurSong();
            //debugger;
            //if (self.eventCallback) self.eventCallback(self.$btnPlaying, "onplay");
            self.toggleBtnPlaying(true);
            sng.$title.addClass("danPlayPauseButtonActive");
            sng.$bar.width(0);
            sng.$prog.addClass("danProgressShow");
        };
        // media is playing after paused or stopped for buffering
        // onplaying -------------------------
        // media has paused
        this.player.onpause = function () {
            //debugger;
            //if (self.eventCallback) self.eventCallback(self.$btnPlaying, "onpause");
            self.toggleBtnPlaying(false);
        };
        // current playlist has ended
        this.player.onended = function () {
            //if (self.eventCallback) self.eventCallback(self.$btnPlaying, "onended");
            //debugger;
            self.setCurrentStopped();
            if (self.autoPlay)
                self.playThis(self.album.gotoNextSong());
            //let $b = $(self.$btnPlaying).find("b");
            //$b.removeClass('danPlayPauseButtonActive')
        };
        // media has advanced
        this.player.ontimeupdate = function () {
            var sng = self.album.getCurSong();
            //debugger;
            var currSecs = parseInt(self.player.currentTime);
            var totSecs = sng.secsDur;
            sng.$bar.css("width", (Math.round((currSecs / totSecs) * 100)) + "%");
            //if (self.eventCallback) self.eventCallback(self.$btnPlaying, "ontimeupdate", secs);
        };
        // browser started looking for media
        //this.player.onloadstart = function () {
        //debugger;
        //}
        // browser can start playing
        //this.player.oncanplay = function () {
        //    debugger;
        //}
        $(".danPlayPauseButton").click(function () {
            //debugger;
            if ($(this).hasClass("danPlayPauseAllButton")) {
                self.autoPlay = true; // clicked album-level play
                if (self.player.paused)
                    self.player.play();
                else
                    self.player.pause();
            }
            else {
                var trackNum = $(this).data("tracknum");
                //let clkSong = self.album.getSong(trackNum);
                var sng = self.album.getCurSong();
                if (sng.trackNum != trackNum) {
                    self.setCurrentStopped();
                    self.playThis(sng);
                }
                else {
                    if (self.player.paused)
                        self.player.play();
                    else
                        self.player.pause();
                }
            }
        });
    }
    DanAudioMgr.prototype.playThis = function (sng) {
        var playName = "/media/songs/" + sng.fileName;
        this.player.src = playName;
        this.player.play();
    };
    DanAudioMgr.prototype.setCurrentStopped = function () {
        var cs = this.album.getCurSong();
        cs.$title.removeClass("danPlayPauseButtonActive");
        cs.$button.removeClass("danPlayPauseButtonActive");
        cs.$btnGlyph.removeClass("glyphicon-pause");
        cs.$btnGlyph.addClass("glyphicon-play");
        cs.$prog.removeClass("danProgressShow");
    };
    DanAudioMgr.prototype.toggleBtnPlaying = function (on) {
        var $b = this.album.getCurSong().$button.find(".glyphicon").first();
        var $masterB = this.album.$album.find(".glyphicon").first();
        if (on) {
            $masterB.removeClass("glyphicon-play");
            $masterB.addClass("glyphicon-pause");
            $b.removeClass("glyphicon-play");
            $b.addClass("glyphicon-pause");
        }
        else {
            $masterB.removeClass("glyphicon-pause");
            $masterB.addClass("glyphicon-play");
            $b.removeClass("glyphicon-pause");
            $b.addClass("glyphicon-play");
        }
    };
    return DanAudioMgr;
}());
