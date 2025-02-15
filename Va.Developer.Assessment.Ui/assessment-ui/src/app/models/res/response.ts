export interface Response<T> {
    message: string;
    succeeded: boolean;
    data: T;
}