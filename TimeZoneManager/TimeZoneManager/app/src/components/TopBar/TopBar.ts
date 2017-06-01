import Vue from 'vue';
import { Component, Prop } from 'vue-property-decorator';
import User from './../../services/User';

@Component
export default class TopBar extends Vue {
    public logout(): void {
        User.logout();
        this.$store.commit('auth/authenticate', false);
        this.$router.push('/login');
    }

    get user(): User {
        return this.$store.getters['auth/appUser'];
    }

    public showSideBar(): void {
        this.$store.commit('sideBar/setMobileVisible', true);
    }
}
