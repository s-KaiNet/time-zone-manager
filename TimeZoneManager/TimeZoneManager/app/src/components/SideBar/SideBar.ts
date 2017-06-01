import Vue from 'vue';
import { Component } from 'vue-property-decorator';

@Component
export default class SideBar extends Vue {
    get isMobileVisible(): boolean {
        return this.$store.getters['sideBar/isMobileVisible'];
    }

    public navigate(location: string): void {
        this.$router.push(location);

        if (this.isMobileVisible) {
            this.$store.commit('sideBar/setMobileVisible', false);
        }
    }
}
