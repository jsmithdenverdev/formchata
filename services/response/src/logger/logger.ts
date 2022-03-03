import { createLogger as createWinstonLogger, format, Logger, transports } from 'winston';

const { printf, combine, timestamp } = format;

export function createLogger(level: string = 'info', prefix: string) {
    return createWinstonLogger({
        level: level,
        format: combine(
            printf(({ level, message, timestamp, ...meadata }) => {
                let msg = `${timestamp} [${level}] ${prefix} : ${message}`;

                if (message) {
                    msg += JSON.stringify(meadata);
                }

                return msg;
            }),
            timestamp(),
        ),
        transports: [new transports.Console()],
    });
}
