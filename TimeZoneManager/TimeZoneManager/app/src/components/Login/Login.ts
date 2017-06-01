import Vue from 'vue';
import { Component } from 'vue-property-decorator';
import jwtDecode from 'jwt-decode';
import User from './../../services/User';

@Component
export default class Login extends Vue {
    public loginForm: any = {
        name: '',
        password: ''
    };
    public registered: boolean = false;
    public rules: any = {
        name: [
            { required: true, message: 'Please enter Login name', trigger: 'blur' }
        ],
        password: [
            { required: true, message: 'Please enter password', trigger: 'blur' }
        ]
    };

    public authenticate(): void {
        let user: User = new User();
        this.$store.commit('auth/appUser', user);
        this.$store.commit('auth/authenticate', true);
    }

    public submitForm(formName: string): void {
        (<any>this.$refs[formName]).validate((valid) => {
            if (!valid) {
                return false;
            }
            let postStr: string = 'grant_type=password&client_id=4c6e21f4-ee5e-4482-a060-0220622911bd&username=' +
                encodeURIComponent(this.loginForm.name) + '&password=' + encodeURIComponent(this.loginForm.password);

            this.$http.post('/connect/token', postStr, {
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded'
                }
            })
                .then(res => {
                    let result: any = jwtDecode(res.data['access_token']);
                    User.save(result, res.data['expires_in'], res.data['access_token']);
                    this.authenticate();

                    this.$router.push('/');
                }, err => {
                    if (err && err.response && err.response.data && err.response.data.error_description === 'Invalid user credentials.') {
                        (<any>this).$message({
                            showClose: true,
                            message: 'Your login name or password is incorrect. Please enter valid credentials.',
                            type: 'warning',
                            duration: 0
                        });
                    } else {
                        (<any>this).$notify.error({
                            title: 'Error',
                            message: 'Something went wrong. Try again later or contact system administrator'
                        });
                        this.$logger.error(err);
                    }
                });
        });
    }

    public register(): void {
        this.$router.push('/register');
    }

    public created(): void {
        if (this.$route.query.registered === 'true') {
            this.registered = true;
        }
    }
}
