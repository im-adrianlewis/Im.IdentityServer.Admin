import moment from 'moment';

export default class Timer {
    private _time: moment.Moment | undefined = undefined;
    private _timer: NodeJS.Timer | undefined = undefined;
    private _initialSeconds: moment.MomentInput;
    private _listener: (remains: number) => void;
    private _timeoutListener: Function | undefined;

    constructor(seconds: number, listener: (remainingTime: number) => void, timeoutListener?: Function) {
        this._initialSeconds = seconds;
        this._listener = listener;
        this._timeoutListener = timeoutListener;
        this.reset();
    }

    start() {
        this._timer = setInterval(_ => {
            if (typeof this._time === 'undefined')
                return;

            this._time.add(-1000);
            this._listener(this._time.valueOf());

            if (this._time.valueOf() <= 0) {
                if (this._timeoutListener) {
                    this._timeoutListener();
                }
                this.end();
            }
        }, 1000);
    }

    end() {
        if (typeof this._timer !== 'undefined') {
            clearInterval(this._timer);
        }

        this.reset();
    }

    reset() {
        this._time = moment(this._initialSeconds);
    }
}

