<template>
  <div v-loading="updating">
    <el-input class="filter" placeholder="Filter by name" icon="search" v-model="filter" @change="onFilterChanged">
    </el-input>
    <el-table :data="timeZones" border style="width: 100%">
      <el-table-column label="Name">
        <template scope="scope">
          <span style="margin-left: 10px">{{ scope.row.name }}</span>
        </template>
      </el-table-column>
      <el-table-column label="City">
        <template scope="scope">
          <el-tag>{{ scope.row.city }}</el-tag>
        </template>
      </el-table-column>
      <el-table-column label="GMT difference">
        <template scope="scope">
          <span>GMT {{ scope.row.offsetText }}</span>
        </template>
      </el-table-column>
      <el-table-column label="Current time">
        <template scope="scope">
          <el-icon name="time"></el-icon>
          <span>{{ scope.row.currentTime }}</span>
        </template>
      </el-table-column>
       <el-table-column label="Owner">
        <template scope="scope">
          <span>{{ scope.row.ownerName }}</span>
        </template>
      </el-table-column>
      <el-table-column width="150">
        <template scope="scope">
          <el-button size="small" @click="updateTimeZone(scope.$index, scope.row)">Edit</el-button>
          <el-button size="small" type="danger" @click="deleteTimeZone(scope.$index, scope.row)">Delete</el-button>
        </template>
      </el-table-column>
    </el-table>
    <div class="paging">
      <el-pagination @size-change="pageSizeChanged" @current-change="pageChanged" :current-page="page" :page-size="pageSize" :page-sizes="[5, 10, 20, 30]" :total="total" layout="sizes, prev, pager, next">
      </el-pagination>
    </div>
    <div>
      <el-button type="primary" class="btn-new" icon="plus" @click="createNewTimeZone">Create New</el-button>
    </div>
    <el-dialog :title="dialogTitle" v-model="showDialog" size="tiny" :lock-scroll="false">
      <el-form :model="timeZoneForm" :rules="rules" ref="timeZoneForm" v-loading="dialogUpdating">
        <el-form-item label="Name" label-width="100" prop="name">
          <el-input v-model="timeZoneForm.name"></el-input>
        </el-form-item>
        <el-form-item label="City" label-width="100" prop="city">
          <el-input v-model="timeZoneForm.city"></el-input>
        </el-form-item>
        <el-form-item label="GMT difference" label-width="100" prop="offset">
          <el-input-number v-model="timeZoneForm.offset" :min="-24" :max="24"></el-input-number>
        </el-form-item>
      </el-form>
      <span slot="footer" class="dialog-footer">
        <el-button @click="closeDialog">Cancel</el-button>
        <el-button type="primary" @click="saveTimeZone('timeZoneForm')">Save</el-button>
      </span>
    </el-dialog>
  </div>
</template>

<script lang="ts" src="./UserTimeZones"></script>

<style lang="scss" scoped>
.btn-new {
  margin: 10px 0 10px;
}

.paging {
  margin: 10px 0 0;
  float: right;
}

.filter {
  margin-bottom: 10px;
  width: 200px;
}
</style>
