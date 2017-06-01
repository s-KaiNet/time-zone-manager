import Vue from 'vue';
import { Component } from 'vue-property-decorator';
import UserTimeZones from './../UserTimeZones/UserTimeZones.vue';

@Component({
    components: {
        'user-time-zones': UserTimeZones
    }
})
export default class TimeZonesManagement extends Vue {
    public ownRecords: string = 'Your own';
    public allRecords: string = 'All';

    public onSwitchTab(tab: any): void {
        if (tab.label === this.ownRecords) {
            (<any>this.$refs)['own'].refresh();
        }

        if (tab.label === this.allRecords) {
            (<any>this).$refs['all'].refresh();
        }
    }
}
