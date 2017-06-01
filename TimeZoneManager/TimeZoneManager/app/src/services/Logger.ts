export interface ILogger {
    info(info: any): void;
    error(error: any): void;
}

export default class Logger implements ILogger {
    public info(info: any): void {
        if (window.console && window.console.log) {
            window.console.log(info);
        }
    }

    public error(error: any): void {
        if (window.console && window.console.error) {
            window.console.error(error);
        }
    }
}

