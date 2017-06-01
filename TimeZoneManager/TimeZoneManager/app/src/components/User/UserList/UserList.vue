<template>
    <div v-loading="updating">
        <el-table :data="users" border style="width: 80%">
            <el-table-column width="75">
                <template scope="scope">
                    <i class="el-icon-edit" @click="editUser(scope.$index, scope.row)"></i>&nbsp;&nbsp;
                    <i class="el-icon-delete" @click="deleteUser(scope.$index, scope.row)"></i>
                </template>
            </el-table-column>
            <el-table-column label="Display name">
                <template scope="scope">
                    <el-tag>{{ scope.row.displayName }}</el-tag>
                </template>
            </el-table-column>
            <el-table-column label="Login name">
                <template scope="scope">
                    <span>{{ scope.row.loginName }}</span>
                </template>
            </el-table-column>
            <el-table-column label="Email">
                <template scope="scope">
                    <el-icon name="message"></el-icon>&nbsp;
                    <span>{{ scope.row.email }}</span>
                </template>
            </el-table-column>
            <el-table-column label="Permissions" v-if="hasPermission('admin')">
                <template scope="scope">
                    <span v-for="permision in scope.row.roleClaims">{{permision.claimValue}} </span>
                </template>
            </el-table-column>
        </el-table>
        <div class="paging">
            <el-pagination @size-change="pageSizeChanged" @current-change="pageChanged" :current-page="page" :page-size="pageSize" :page-sizes="[5, 10, 20, 30]" :total="total" layout="sizes, prev, pager, next">
            </el-pagination>
        </div>
        <div>
            <el-button type="primary" class="btn-new" icon="plus" @click="createNewUser">Create New</el-button>
        </div>
    </div>
</template>

<script lang="ts" src="./UserList"></script>

<style lang="scss" scoped>
.btn-new {
    margin: 10px 0 10px;
}

.paging {
    margin: 10px 0 0;
}

i {
    cursor: pointer;
}
</style>
