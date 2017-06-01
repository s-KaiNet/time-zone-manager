<template>
    <div>
        <div v-if="registered">
            You have successfully registered. Please use your login and password in order to enter the site.
        </div>
        <br>
        <el-row>
            <el-col :sm="12" :xs="24" :md="8">
                <div class="grid-content">
                    <el-form :model="loginForm" :rules="rules" ref="loginForm" label-width="120px">
                        <el-form-item label="Login name" prop="name">
                            <el-input placeholder="Enter login name" v-model="loginForm.name"></el-input>
                        </el-form-item>
                        <el-form-item label="Password" prop="password">
                            <el-input placeholder="Enter password" type="password" v-model="loginForm.password"></el-input>
                        </el-form-item>
                        <el-form-item>
                            <el-button type="primary" @click="submitForm('loginForm')">Login</el-button>
                            <span>OR</span>
                            <el-button type="text" @click="register">Register</el-button>
                        </el-form-item>
                    </el-form>
                </div>
            </el-col>
        </el-row>
    </div>
</template>

<script lang="ts" src="./Login"></script>

<style lang="scss">

</style>
