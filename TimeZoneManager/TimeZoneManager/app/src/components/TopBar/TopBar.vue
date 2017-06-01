<template>
    <div class="topbar">
        <div class="text side-menu">
            <i class="el-icon-menu" @click="showSideBar"></i>
        </div>
        <div class="text">Time Zone Manager App</div>
        <div v-if="isAuthenticated" class="text login">Welcome, {{user.displayName}}
            <img src="/static/avatar.png" />&nbsp;
            <i class="fa fa-power-off" @click="logout"></i>
        </div>
    </div>
</template>

<script lang="ts" src="./TopBar"></script>
<style lang="scss">
.topbar {
    height: 64px;
    background-color: rgb(9, 142, 199);

    .text {
        font-size: 20px;
        color: white;
        font-weight: 400;
        padding: 20px;
        float: left;
    }
    .login {
        float: right;

        img {
            vertical-align: middle;
            height: 34px;
            border-radius: 17px;
        }

        i {
            cursor: pointer;
        }
    }
}
</style>