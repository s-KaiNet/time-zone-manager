import User from './../../services/User';

interface IAuthState {
    authenticated: boolean;
    appUser: User;
}

const state: IAuthState = {
    authenticated: false,
    appUser: null
};

const getters: any = {
    isAuthenticated: (authState: IAuthState) => {
        return authState.authenticated;
    },
    appUser: (authState: IAuthState) => {
        return authState.appUser;
    }
};

const mutations: any = {
    authenticate: (authState: IAuthState, isAuthenticated: boolean) => {
        authState.authenticated = isAuthenticated;
    },
    appUser: (authState: IAuthState, user: User) => {
        authState.appUser = user;
    }
};

export default {
    state: state,
    getters: getters,
    mutations: mutations,
    namespaced: true
};
