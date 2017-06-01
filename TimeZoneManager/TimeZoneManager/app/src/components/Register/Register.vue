<template>
    <div>
        <el-row>
            <el-col :sm="12" :xs="24" :md="8">
                <div class="grid-content">
                    <el-form :model="registerForm" :rules="rules" ref="registerForm">
                        <el-form-item label="Login name" prop="name">
                            <el-input placeholder="Enter login name" v-model="registerForm.name"></el-input>
                        </el-form-item>
                        <el-form-item label="Email" prop="email">
                            <el-input placeholder="Enter email" type="email" v-model="registerForm.email"></el-input>
                        </el-form-item>
                        <el-form-item label="Password" prop="password">
                            <el-input type="password" placeholder="Enter password" v-model="registerForm.password"></el-input>
                        </el-form-item>
                        <el-form-item label="Re-enter password" prop="checkPassword">
                            <el-input placeholder="Enter password once again" type="password" v-model="registerForm.checkPassword"></el-input>
                        </el-form-item>
                        <el-form-item label="Display name" prop="displayName">
                            <el-input placeholder="Enter display name" v-model="registerForm.displayName"></el-input>
                        </el-form-item>
                    </el-form>
                </div>
            </el-col>
            <el-col :xs="24" :md="8" :sm="12">
                <div class="grid-content error-info" v-if="hasError">
                    <div>Please, correct below errors:</div>
                    <br>
                    <ul>
                        <li v-for="error in errors">
                            {{ error }}
                        </li>
                    </ul>
                </div>
            </el-col>
        </el-row>
        <el-row>
            <el-col :xs="24" :md="8">
                <el-button type="primary" @click="submitForm('registerForm')">Register</el-button>
                <el-button @click="cancel()">Cancel</el-button>
            </el-col>
        </el-row>
    </div>
</template>

<script lang="ts" src="./Register"></script>

<style lang="scss">
.error-info {
    margin: 10px 0 0 40px;
    color: red;

    ul li {
        list-style-type: circle;
    }
}
</style>
