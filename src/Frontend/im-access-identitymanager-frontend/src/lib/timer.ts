import moment from 'moment';

export default class Timer {
  private _time?: moment.Moment;
  private _timer?: NodeJS.Timer;
  private _initialSeconds: moment.MomentInput;
  private _listener: (remains: number) => void;
  private _timeoutListener?: Function;

  constructor(seconds: number, listener: (remainingTime: number) => void, timeoutListener?: Function) {
    this._initialSeconds = seconds;
    this._listener = listener;
    this._timeoutListener = timeoutListener;

    this.reset();
  }

  public start() {
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

  public end() {
    if (typeof this._timer !== 'undefined') {
      clearInterval(this._timer);
    }

    this.reset();
  }

  public reset() {
    this._time = moment(this._initialSeconds);
  }
}
