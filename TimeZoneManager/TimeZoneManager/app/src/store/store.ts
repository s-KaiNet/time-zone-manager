import Vue from 'vue';
import Vuex from 'vuex';
import auth from './modules/auth';
import sideBar from './modules/sideBar';

Vue.use(Vuex);

export const store: Vuex.Store<any> = new Vuex.Store({
    modules: {
        auth: auth,
        sideBar: sideBar
    }
});
