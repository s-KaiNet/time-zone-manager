import Vue from 'vue';
import User from './services/User';
import { ILogger } from './services/Logger';
import Logger from './services/Logger';

Vue.mixin({
    created(): void {
        let logger: ILogger = new Logger();
        this.$logger = logger;
    },
    computed: {
        isAuthenticated(): boolean {
            if (!this.$store) {
                return false;
            }

            return this.$store.getters['auth/isAuthenticated'];
        }
    },
    methods: {
        hasPermission(permission: string): boolean {
            let user: User = this.$store.getters['auth/appUser'];
            if (!user.isAuthenticated()) {
                return false;
            }

            return user.permissions.indexOf(permission) !== -1;
        }
    }
});
