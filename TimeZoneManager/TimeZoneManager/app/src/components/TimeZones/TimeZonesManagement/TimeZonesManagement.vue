<template>
    <div>
        <div v-if="hasPermission('admin')">
            <el-tabs type="card" @tab-click="onSwitchTab">
                <el-tab-pane :label="ownRecords">
                    <user-time-zones ref="own"></user-time-zones>
                </el-tab-pane>
                <el-tab-pane :label="allRecords">
                    <user-time-zones :show-all="true" ref="all"></user-time-zones>
                </el-tab-pane>
            </el-tabs>
        </div>
        <div v-else>
            <user-time-zones></user-time-zones>
        </div>
    </div>
</template>

<script lang="ts" src="./TimeZonesManagement"></script>

<style>

</style>
