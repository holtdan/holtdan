/// <reference path="typings/jquery/jquery.d.ts" />
var DanAudioMgr = (function () {
    function DanAudioMgr(audioPlayerID, eventCallback) {
        this.audioPlayerID = audioPlayerID;
        this.eventCallback = eventCallback;
        this.player = document.getElementById(audioPlayerID);
        var self = this;
        this.player.onplay = function () {
            //debugger;
            if (self.eventCallback)
                self.eventCallback(self.$btnPlaying, "onplay");
            self.toggleBtnPlaying(true);
        };
        this.player.onpause = function () {
            //debugger;
            if (self.eventCallback)
                self.eventCallback(self.$btnPlaying, "onpause");
            self.toggleBtnPlaying(false);
        };
        this.player.onended = function () {
            if (self.eventCallback)
                self.eventCallback(self.$btnPlaying, "onended");
            self.toggleBtnPlaying(false);
            self.playing = "";
            var $b = $(self.$btnPlaying).find("b");
            $b.removeClass('danPlayPauseButtonActive');
        };
        $(".danPlayPauseButton").click(function () {
            //debugger;
            self.$btnPlaying = $(this);
            var fName = $(this).data("fname"); //this.id;
            //if (makeID(fName) == playing) {
            if (fName == self.playing) {
                if (self.player.paused)
                    self.player.play();
                else
                    self.player.pause();
            }
            else {
                var playName = "/media/songs/" + fName;
                self.player.src = playName;
                self.playing = fName;
                self.player.play();
                var $b = $(self.$btnPlaying).find("b");
                $(".danPlayPauseButtonActive").removeClass("danPlayPauseButtonActive");
                $b.addClass('danPlayPauseButtonActive');
            }
        });
    }
    DanAudioMgr.prototype.toggleBtnPlaying = function (on) {
        var $b = $(this.$btnPlaying).find("b");
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
