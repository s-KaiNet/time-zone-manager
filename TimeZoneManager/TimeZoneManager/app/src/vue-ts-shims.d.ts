import Vue from "vue";
import User from './services/User';
import {ILogger} from './services/Logger';

declare module "vue/types/vue" {
  interface Vue {
    $http: Axios.AxiosInstance;
    $axios: Axios.AxiosInstance;
    $appUser: User;
    $logger: ILogger;
    hasPermission(permission: string): boolean;
  }
}