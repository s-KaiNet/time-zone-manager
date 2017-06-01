import store from 'store/dist/store.modern';

export interface IUserData {
    id: string;
    permissions: string[];
    loginName: string;
    displayName: string;
    token: string;
    isAuthenticated: () => boolean;
}

interface IStoreData extends IUserData {
    timestamp: Date;
}

export default class User implements IUserData {
    private static userKey: string = '_tmz_user_';
    public permissions: string[];
    public loginName: string;
    public displayName: string;
    public token: string;
    public id: string;

    public static save(jwtData: any, expiresIn: number, token: string): void {
        let expiration: Date = new Date();
        expiration.setSeconds(expiration.getSeconds() + expiresIn - 60);

        if (typeof jwtData['urn:permission'] === 'string') {
            jwtData['urn:permission'] = [jwtData['urn:permission']];
        }

        store.set(User.userKey, <IStoreData>{
            permissions: jwtData['urn:permission'],
            loginName: jwtData['sub'],
            displayName: jwtData['urn:name'],
            token: token,
            id: jwtData['urn:id'],
            timestamp: expiration
        }, expiration);
    }

    public static logout(): void {
        store.remove(this.userKey);
    }

    constructor() {
        let data: IStoreData = <IStoreData>store.get(User.userKey);

        if (!data) {
            return;
        }

        let currentTime: Date = new Date();
        let expirationTime: Date = new Date(data.timestamp);

        if (expirationTime < currentTime) {
            store.remove(User.userKey);
            data = null;
        }

        if (data) {
            this.permissions = data.permissions;
            this.loginName = data.loginName;
            this.displayName = data.displayName;
            this.token = data.token;
            this.id = data.id;
        }
    }

    public isAuthenticated(): boolean {
        return !!this.token;
    }
}
