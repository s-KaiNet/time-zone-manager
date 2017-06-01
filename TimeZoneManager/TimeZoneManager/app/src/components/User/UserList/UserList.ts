import Vue from 'vue';
import { Component } from 'vue-property-decorator';
import { IUser } from './../../../interfaces/IUser';
import User from './../../../services/User';

@Component
export default class UserList extends Vue {
    public updating: boolean = false;
    public page: number = 1;
    public pageSize: number = 5;
    public total: number = 0;
    public users: IUser[] = [];

    public deleteUser(index: number, data: IUser): void {

        (<any>this).$confirm(`Delete user '${data.displayName}'?`, 'Warning', {
            confirmButtonText: 'Yes',
            cancelButtonText: 'No',
            type: 'warning'
        })
            .then(() => {
                this.$http.delete(`/api/users/${data.loginName}`)
                    .then(res => {
                        (<any>this).$message({
                            message: 'Operation completed',
                            type: 'success'
                        });
                        this.page = 1;
                        this.queryUsers();
                    },
                    err => {
                        (<any>this).$notify.error({
                            title: 'Error',
                            message: 'Something went wrong. Try again later or contact system administrator'
                        });
                        this.$logger.error(err);
                    });
            })
            .catch(() => {/* */ });
    }

    public editUser(index: number, data: IUser): void {
        this.$router.push({
            name: 'editUser',
            params: {
                userName: data.loginName
            }
        });
    }

    public pageSizeChanged(pageSize: number): void {
        this.pageSize = pageSize;
        this.updating = true;
        this.queryUsers();
    }

    public pageChanged(page: number): void {
        this.page = page;
        this.updating = true;
        this.queryUsers();
    }

    public createNewUser(): void {
        this.$router.push('users/new');
    }

    /* lifecycle */
    public created(): void {
        this.queryUsers();
    }

    private queryUsers(): void {
        this.updating = true;
        this.$http.get('/api/users', {
            params: {
                page: this.page,
                pageSize: this.pageSize
            }
        })
            .then(res => {
                this.updating = false;
                let data: any = (<any>res.data).users;
                this.users = data;
                this.total = (<any>res.data).total;
            }, err => {
                this.updating = false;
                (<any>this).$notify.error({
                    title: 'Error',
                    message: 'Something went wrong. Try again later or contact system administrator'
                });
                this.$logger.error(err);
            });
    }
}
