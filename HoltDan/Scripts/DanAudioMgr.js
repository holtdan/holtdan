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
        this.songs = $.map($songs, function (item) {
            var $t = $(item);
            var $btn = $t.find(".danPlayPauseButton").first();
            var sng = {
                $danSong: $t,
                $title: $t.find(".danTitle").first(),
                $button: $btn,
                $btnGlyph: $btn.find("b"),
                fileName: $btn.data("fname"),
                secsDur: parseInt($btn.data("secsdur")),
                $prog: $t.find(".danProgress").first(),
                $bar: $t.find(".danProgressBar").first()
            };
            return sng;
        });
    }
    DanSongAlbum.prototype.ButtonIsCurrent = function (button) {
        if (!this.curSong)
            return false;
        var inFN = button.data("fname");
        return this.curSong.fileName == inFN;
    };
    DanSongAlbum.prototype.GetSongFromButton = function (button) {
        var self = this;
        var inFN = button.data("fname");
        var one = jQuery.grep(self.songs, function (item, idx) {
            var itmFN = item.$button.data("fname");
            return itmFN == inFN;
        });
        return one[0];
    };
    return DanSongAlbum;
}());
var DanAudioMgr = (function () {
    function DanAudioMgr(audioPlayerID, danSongAlbumSelector, eventCallback) {
        this.audioPlayerID = audioPlayerID;
        this.danSongAlbumSelector = danSongAlbumSelector;
        this.eventCallback = eventCallback;
        this.album = new DanSongAlbum(danSongAlbumSelector);
        this.player = document.getElementById(audioPlayerID);
        var self = this;
        this.player.onplay = function () {
            var sng = self.album.curSong;
            //debugger;
            //if (self.eventCallback) self.eventCallback(self.$btnPlaying, "onplay");
            self.toggleBtnPlaying(true);
            sng.$title.addClass("danPlayPauseButtonActive");
            sng.$bar.width(0);
            sng.$prog.addClass("danProgressShow");
        };
        this.player.onpause = function () {
            //debugger;
            //if (self.eventCallback) self.eventCallback(self.$btnPlaying, "onpause");
            self.toggleBtnPlaying(false);
        };
        this.player.onended = function () {
            //if (self.eventCallback) self.eventCallback(self.$btnPlaying, "onended");
            self.reset();
            //let $b = $(self.$btnPlaying).find("b");
            //$b.removeClass('danPlayPauseButtonActive')
        };
        this.player.ontimeupdate = function () {
            var sng = self.album.curSong;
            //debugger;
            var currSecs = parseInt(self.player.currentTime);
            var totSecs = sng.secsDur;
            sng.$bar.css("width", (Math.round((currSecs / totSecs) * 100)) + "%");
            //if (self.eventCallback) self.eventCallback(self.$btnPlaying, "ontimeupdate", secs);
        };
        this.player.onloadstart = function () {
            debugger;
        };
        this.player.oncanplay = function () {
            debugger;
        };
        $(".danPlayPauseButton").click(function () {
            var sng = self.album.GetSongFromButton($(this));
            if (!self.album.curSong || self.album.curSong.fileName != sng.fileName) {
                self.reset();
                var playName = "/media/songs/" + sng.fileName;
                self.album.curSong = sng;
                self.player.src = playName;
                self.player.play();
            }
            else {
                if (self.player.paused)
                    self.player.play();
                else
                    self.player.pause();
            }
        });
    }
    DanAudioMgr.prototype.reset = function () {
        if (this.album.curSong) {
            var cs = this.album.curSong;
            cs.$title.removeClass("danPlayPauseButtonActive");
            cs.$button.removeClass("danPlayPauseButtonActive");
            cs.$btnGlyph.removeClass("glyphicon-pause");
            cs.$btnGlyph.addClass("glyphicon-play");
            cs.$prog.removeClass("danProgressShow");
            this.album.curSong = null;
        }
    };
    DanAudioMgr.prototype.toggleBtnPlaying = function (on) {
        var $b = this.album.curSong.$button.find(".glyphicon").first();
        if (on) {
            $b.removeClass("glyphicon-play");
            $b.addClass("glyphicon-pause");
        }
        else {
            $b.removeClass("glyphicon-pause");
            $b.addClass("glyphicon-play");
        }
    };
    return DanAudioMgr;
}());
//# sourceMappingURL=DanAudioMgr.js.map