import Vue from 'vue';
import App from './components/App/App.vue';
import * as ElementUI from 'element-ui';
import VueRouter from 'vue-router';
import http from './http';
import VueAxios from 'vue-axios';
import 'element-ui/lib/theme-default/index.css';
import locale from 'element-ui/lib/locale/lang/en';
import 'font-awesome/css/font-awesome.css';
import 'nprogress/nprogress.css';
import router from './routes';
import './mixins';
import {store} from './store/store';

Vue.use(ElementUI, { locale });
Vue.use(VueRouter);
Vue.use(VueAxios, http);

const app: any = new Vue({
    el: '#app',
    render: h => h(App),
    store: store,
    router: router
});
