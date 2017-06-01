<template>
  <div>
        <el-row>
            <el-col :sm="12" :xs="24" :md="8">
                <div class="grid-content">
                    <el-form :model="editForm" :rules="rules" ref="editForm">
                        <el-form-item label="Login name">
                            <el-input v-model="editForm.loginName" disabled></el-input>
                        </el-form-item>
                        <el-form-item label="Email" prop="email">
                            <el-input placeholder="Enter email" type="email" v-model="editForm.email"></el-input>
                        </el-form-item>
                        <el-form-item label="Display name" prop="displayName">
                            <el-input placeholder="Enter display name" v-model="editForm.displayName"></el-input>
                        </el-form-item>
                        <el-form-item label="Role" prop="role" v-if="hasPermission('admin')">
                            <el-radio-group v-model="editForm.role">
                                <el-radio label="user">User</el-radio>
                                <el-radio label="manager">Manager</el-radio>
                                <el-radio label="admin">Admin</el-radio>
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
                <el-button type="primary" @click="submitForm('editForm')">Update</el-button>
                <el-button @click="cancel()">Cancel</el-button>
            </el-col>
        </el-row>
    </div>
</template>

<script lang="ts" src="./Edit">
</script>

<style>

</style>
