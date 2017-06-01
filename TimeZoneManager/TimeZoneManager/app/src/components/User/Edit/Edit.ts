import Vue from 'vue';
import { Component, Prop } from 'vue-property-decorator';

import { IUser } from './../../../interfaces/IUser';

@Component
export default class Edit extends Vue {
    @Prop()
    public userName: string;

    public editForm: any = {
        loginName: '',
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

            this.$http.put(`api/users/${this.userName}`, this.editForm)
                .then(res => {
                    let data: any = res.data;
                    if (data.succeeded) {
                        this.$router.back();
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

    public created(): void {
        this.$http.get(`api/users/${this.userName}`)
            .then(res => {
                let data: IUser = <IUser>res.data;
                this.editForm.loginName = data.loginName;
                this.editForm.displayName = data.displayName;
                this.editForm.email = data.email;
                this.editForm.role = data.role;
            }, err => {
                (<any>this).$notify.error({
                    title: 'Error',
                    message: 'Something went wrong. Try again later or contact system administrator'
                });
                this.$logger.error(err);
            });
    }
}
