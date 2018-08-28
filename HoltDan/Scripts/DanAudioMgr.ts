/// <reference path="typings/jquery/jquery.d.ts" />

class DanAudioMgr {
    private player: any;
    private playing: string;
    private $btnPlaying: JQuery;

    constructor(public audioPlayerID: string, public eventCallback: (triggerElem: JQuery, eventName: string) => void) {
        this.player = document.getElementById(audioPlayerID);
        let self: DanAudioMgr = this;

        this.player.onplay = function () {
            //debugger;
            if (self.eventCallback) self.eventCallback(self.$btnPlaying, "onplay");
            self.toggleBtnPlaying(true);
        };
        this.player.onpause = function () {
            //debugger;
            if (self.eventCallback) self.eventCallback(self.$btnPlaying, "onpause");
            self.toggleBtnPlaying(false);
        };
        this.player.onended = function () {
            if (self.eventCallback) self.eventCallback(self.$btnPlaying, "onended");
            self.toggleBtnPlaying(false);
            self.playing = "";
            let $b = $(self.$btnPlaying).find("b");
            $b.removeClass('danPlayPauseButtonActive')
        };
        $(".danPlayPauseButton").click(function () {
            //debugger;
            self.$btnPlaying = $(this);
            let fName = $(this).data("fname");//this.id;
            //if (makeID(fName) == playing) {
            if (fName == self.playing) {
                if (self.player.paused)
                    self.player.play();
                else
                    self.player.pause();
            }
            else {
                let playName = "/media/songs/" + fName;
                self.player.src = playName;
                self.playing = fName;
                self.player.play();
                let $b = $(self.$btnPlaying).find("b");
                $(".danPlayPauseButtonActive").removeClass("danPlayPauseButtonActive")
                $b.addClass('danPlayPauseButtonActive')
            }
        })
    }

    private toggleBtnPlaying(on: boolean): void {
        let $b = $(this.$btnPlaying).find("b");
        if (on) {
            $b.removeClass("glyphicon-play");
            $b.addClass("glyphicon-pause");
        }
        else {
            $b.removeClass("glyphicon-pause");
            $b.addClass("glyphicon-play");
        }
    }
}