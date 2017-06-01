<template>
    <div>
        <el-row>
            <el-col :sm="12" :xs="24" :md="8">
                <div class="grid-content">
                    <el-form :model="newUserForm" :rules="rules" ref="newUserForm">
                        <el-form-item label="Login name" prop="loginName">
                            <el-input placeholder="Enter login name" v-model="newUserForm.loginName"></el-input>
                        </el-form-item>
                        <el-form-item label="Email" prop="email">
                            <el-input placeholder="Enter email" type="email" v-model="newUserForm.email"></el-input>
                        </el-form-item>
                        <el-form-item label="Password" prop="password">
                            <el-input type="password" placeholder="Enter password" v-model="newUserForm.password"></el-input>
                        </el-form-item>
                        <el-form-item label="Re-enter password" prop="checkPassword">
                            <el-input placeholder="Enter password once again" type="password" v-model="newUserForm.checkPassword"></el-input>
                        </el-form-item>
                        <el-form-item label="Display name" prop="displayName">
                            <el-input placeholder="Enter display name" v-model="newUserForm.displayName"></el-input>
                        </el-form-item>
                        <el-form-item label="Role" prop="role" v-if="hasPermission('admin')">
                            <el-radio-group v-model="newUserForm.role">
                                <el-radio label="User">User</el-radio>
                                <el-radio label="Manager">Manager</el-radio>
                                <el-radio label="Admin">Admin</el-radio>
                            </el-radio-group>
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
                <el-button type="primary" @click="submitForm('newUserForm')">Create</el-button>
                <el-button @click="cancel()">Cancel</el-button>
            </el-col>
        </el-row>
    </div>
</template>

<script lang="ts" src="./New"></script>

<style lang="scss">
.error-info {
    margin: 10px 0 0 40px;
    color: red;

    ul li {
        list-style-type: circle;
    }
}
</style>
