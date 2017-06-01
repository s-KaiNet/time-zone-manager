interface ISideBarState {
    sideBarMobileVisisble: boolean;
}

const state: ISideBarState = {
    sideBarMobileVisisble: false
};

const getters: any = {
    isMobileVisible: (sideBarState: ISideBarState) => {
        return sideBarState.sideBarMobileVisisble;
    }
};

const mutations: any = {
    setMobileVisible: (sideBarState: ISideBarState, isMobileVisible: boolean) => {
        sideBarState.sideBarMobileVisisble = isMobileVisible;
    }
};

export default {
    state: state,
    getters: getters,
    mutations: mutations,
    namespaced: true
};
