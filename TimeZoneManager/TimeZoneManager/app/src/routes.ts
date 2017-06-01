import Login from './components/Login/Login.vue';
import Register from './components/Register/Register.vue';
import TimeZonesManagement from './components/TimeZones/TimeZonesManagement/TimeZonesManagement.vue';
import UserList from './components/User/UserList/UserList.vue';
import * as EditUser from './components/User/Edit/Edit.vue';
import * as NewUser from './components/User/New/New.vue';
import User from './services/User';

import { RouteConfig } from 'vue-router';
import VueRouter from 'vue-router';

const routes: RouteConfig[] = [
    {
        path: '', component: TimeZonesManagement
    },
    {
        path: '/login', component: Login
    },
    {
        path: '/register', component: Register
    },
    {
        path: '/users', component: UserList
    },
    {
        path: '/users/new', component: NewUser
    },
    {
        path: '/users/edit/:userName', component: EditUser, props: true, name: 'editUser'
    }
];

let router: VueRouter = new VueRouter({
    routes: routes,
    mode: 'history'
});

router.beforeEach((to, from, next) => {
    let user: User = new User();
    if (to.path !== '/login' && to.path !== '/register' && !user.isAuthenticated()) {
        next('/login');
        return;
    }

    next();
});

export default router;
