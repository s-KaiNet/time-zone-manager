import Vue from 'vue';
import { Component } from 'vue-property-decorator';

import SideBar from './../SideBar/SideBar.vue';
import TopBar from './../TopBar/TopBar.vue';
import User from './../../services/User';

@Component({
  components: {
    'side-bar': SideBar,
    'top-bar': TopBar
  }
})
export default class App extends Vue {
  public created(): void {
    let user: User = new User();
    this.$store.commit('auth/appUser', user);
    this.$store.commit('auth/authenticate', user.isAuthenticated());
  }
}
