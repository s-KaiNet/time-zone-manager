import Vue from 'vue';
import { Component, Prop } from 'vue-property-decorator';
import { ITimeZone } from './../../../interfaces/ITimeZone';
import User from './../../../services/User';

@Component
export default class UserTimeZones extends Vue {

    @Prop()
    public showAll: boolean;

    public timeZones: ITimeZone[] = [];
    public currentTimeZone: ITimeZone = null;
    public showDialog: boolean = false;
    public updating: boolean = false;
    public dialogUpdating: boolean = false;
    public page: number = 1;
    public pageSize: number = 5;
    public total: number = 0;
    public filter: string = '';
    public timeout?: number = null;

    public timeZoneForm: any = {
        name: '',
        city: '',
        offset: 0
    };

    public rules: any = {
        name: [
            { required: true, message: 'Please enter time zone Name', trigger: 'blur' }
        ],
        city: [
            { required: true, message: 'Please enter City', trigger: 'blur' }
        ],
        offset: [
            { required: true, message: 'Please enter time difference from GMT', trigger: 'blur' }
        ]
    };

    public updateTimeZone(index: number, data: ITimeZone): void {
        this.currentTimeZone = data;

        this.timeZoneForm.name = this.currentTimeZone.name;
        this.timeZoneForm.city = this.currentTimeZone.city;
        this.timeZoneForm.offset = this.currentTimeZone.offset;

        this.showDialog = true;
    }

    public createNewTimeZone(): void {
        this.currentTimeZone = null;

        this.timeZoneForm.name = '';
        this.timeZoneForm.city = '';
        this.timeZoneForm.offset = 0;

        this.showDialog = true;
    }

    public deleteTimeZone(index: number, data: ITimeZone): void {

        (<any>this).$confirm(`Delete Time Zone ${data.name}?`, 'Warning', {
            confirmButtonText: 'Yes',
            cancelButtonText: 'No',
            type: 'warning'
        })
            .then(() => {
                this.$http.delete(`/api/timezones/${data.id}`)
                    .then(res => {
                        (<any>this).$message({
                            message: 'Operation completed',
                            type: 'success'
                        });
                        this.page = 1;
                        this.queryTimeZones();
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

    public closeDialog(): void {
        this.showDialog = false;
    }

    public saveTimeZone(): void {
        this.dialogUpdating = true;
        let xhr: Axios.IPromise<Axios.AxiosXHR<{}>> = null;
        if (this.currentTimeZone) {
            xhr = this.$http.put(`/api/timezones/${this.currentTimeZone.id}`, this.timeZoneForm);
        } else {
            xhr = this.$http.post(`/api/timezones/`, this.timeZoneForm);
        }

        xhr.then(res => {
            this.dialogUpdating = false;
            let data: ITimeZone = <ITimeZone>res.data;
            this.showDialog = false;
            if (!this.currentTimeZone) {
                this.page = 1;
            }
            this.queryTimeZones();

            (<any>this).$message({
                message: 'Operation completed',
                type: 'success'
            });

        }, err => {
            this.dialogUpdating = false;
            (<any>this).$notify.error({
                title: 'Error',
                message: 'Something went wrong. Try again later or contact system administrator'
            });
            this.$logger.error(err);
        });
    }

    public onFilterChanged(data: any): void {
        if (this.timeout) {
            clearTimeout(this.timeout);
        }

        this.timeout = setTimeout(() => {
            this.queryTimeZones();
        }, 1500);
    }

    public pageSizeChanged(pageSize: number): void {
        this.pageSize = pageSize;
        this.updating = true;
        this.queryTimeZones();
    }

    public pageChanged(page: number): void {
        this.page = page;
        this.updating = true;
        this.queryTimeZones();
    }

    public refresh(): void {
        this.queryTimeZones();
    }

    get dialogTitle(): string {
        if (this.currentTimeZone) {
            return `Edit Time Zone - ${this.currentTimeZone.name}`;
        } else {
            return 'New Time Zone';
        }
    }

    /* lifecycle */
    public created(): void {
        this.queryTimeZones();
    }

    private queryTimeZones(): void {
        this.updating = true;
        let user: User = this.$store.getters['auth/appUser'];
        let url: string = this.showAll ? `/api/timezones/` : `/api/timezones/${user.id}`;
        this.$http.get(url, {
            params: {
                page: this.page,
                pageSize: this.pageSize,
                filter: this.filter
            }
        })
            .then(res => {
                this.updating = false;
                let data: any = (<any>res.data).timeZones;
                if (!data || data.length === 0) {
                    this.timeZones = data;
                    this.total = 0;
                    return;
                }

                for (let i: number = 0; i < data.length; i++) {
                    this.bindTime(data[i]);
                }

                this.timeZones = data;
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

    private bindTime(timeZone: ITimeZone): void {
        if (timeZone.offset >= 0) {
            timeZone.offsetText = `+${timeZone.offset}`;
        } else {
            timeZone.offsetText = timeZone.offset.toString();
        }
        let offsetDate: Date = this.convertDateToUTC(new Date(timeZone.offsetTime));
        offsetDate.setHours(offsetDate.getHours() + timeZone.offset);
        timeZone.currentTime = `${('0' + offsetDate.getHours()).slice(-2)}:${('0' + offsetDate.getMinutes()).slice(-2)},
        ${offsetDate.toDateString()}`;
    }

    private convertDateToUTC(date: Date): Date {
        return new Date(date.getUTCFullYear(), date.getUTCMonth(), date.getUTCDate(),
            date.getUTCHours(), date.getUTCMinutes(), date.getUTCSeconds());
    }
}
