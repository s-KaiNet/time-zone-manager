<template>
    <div class="sidebar" :class="{'mobile-hide': !isMobileVisible}">
        <div class="brand">
            <div>
                <img src="/static/icon.jpg" />
            </div>
            <div class="title">Time Zone Manager</div>
        </div>
        <el-menu v-if="isAuthenticated">    
            <el-menu-item @click="navigate('/')" index="1">My Time Zones</el-menu-item>
            <el-menu-item @click="navigate('users')" v-if="hasPermission('manager')" index="2">User Management</el-menu-item>
        </el-menu>
    </div>
</template>

<script lang="ts" src="./SideBar"></script>
<style lang="scss">
.sidebar {
    width: 260px;
    background-color: #EEF1F6;
    box-shadow: 0 1px 5px rgba(0, 0, 0, .2), 0 2px 2px rgba(0, 0, 0, .14), 0 3px 1px -2px rgba(0, 0, 0, .12);
    height: 100%;
    position: absolute;
    z-index: 100;

    .brand {
        text-align: center;
        padding: 10px;
        border-bottom: 1px solid #bfbebe;

        .title {
            font-size: 24px;
            color: #02648e;
        }

        img {
            width: 40%;
        }
    }
}
</style>