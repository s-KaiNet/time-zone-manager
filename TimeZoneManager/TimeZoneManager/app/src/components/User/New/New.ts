import Vue from 'vue';
import { Component } from 'vue-property-decorator';

@Component
export default class New extends Vue {
    public newUserForm: any = {
        loginName: '',
        password: '',
        checkPassword: '',
        displayName: '',
        role: ''
    };
    public hasError: boolean = false;
    public errors: string[] = [];
    public rules: any = {
        loginName: [
            {
                required: true, trigger: 'blur', validator: (rule, value, callback) => {
                    if (value !== '' && !/^[a-zA-Z0-9]+$/.test(value)) {
                        callback(new Error('Only letters and digigts allowed'));
                        return;
                    } else if (!value) {
                        callback(new Error('Please enter Login name'));
                        return;
                    }

                    callback();
                }
            }
        ],
        password: [
            { required: true, message: 'Please enter Password', trigger: 'blur' }
        ],
        email: [
            {
                required: true, trigger: 'blur', validator: (rule, value, callback) => {
                    if (value !== '' && !/\S+@\S+\.\S+/.test(value)) {
                        callback(new Error('Please enter valid email address'));
                        return;
                    } else if (!value) {
                        callback(new Error('Please enter email'));
                        return;
                    }

                    callback();
                }
            }
        ],
        checkPassword: [
            {
                required: true, trigger: 'blur', validator: (rule, value, callback) => {
                    if (value !== '' && value !== this.newUserForm.password) {
                        callback(new Error('Passwords entered don\'t match'));
                        return;
                    } else if (!value) {
                        callback(new Error('Please re-enter Password'));
                        return;
                    }

                    callback();
                }
            }
        ],
        displayName: [
            { required: true, message: 'Please enter Display name', trigger: 'blur' }
        ],
        role: [
            { required: true, message: 'Please select a role', trigger: 'blur' }
        ]
    };

    public submitForm(formName: string): void {
        this.hasError = false;

        (<any>this.$refs[formName]).validate((valid) => {
            if (!valid) {
                return false;
            }

            this.$http.post('api/users', this.newUserForm)
                .then(res => {
                    let data: any = res.data;
                    if (data.succeeded) {
                        this.$router.back();
                        return;
                    }

                    if (data.userExists) {
                        this.hasError = true;
                        this.errors = ['The user with specified name exists. Please select another name'];
                        return;
                    }

                    if (!data.succeeded && typeof data.succeeded === 'boolean' && data.errors) {
                        this.hasError = true;
                        this.errors = [];
                        for (let i: number = 0; i < data.errors.length; i++) {
                            this.errors.push(data.errors[i].description);
                        }
                        return;
                    }

                }, err => {
                    (<any>this).$notify.error({
                        title: 'Error',
                        message: 'Something went wrong. Try again later or contact system administrator'
                    });
                    this.$logger.error(err);
                });
        });
    }

    public cancel(): void {
        this.$router.back();
    }
}
