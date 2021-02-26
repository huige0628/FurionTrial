<template>
    <div>
        <!-- head -->
        <SimpleHead
            :model="filter"
            @on-search="reloadTable(true)"
            @on-expand="onExpand"
        >
            <template slot="button">
                <!-- 操作按钮 -->
                <el-form-item>
                    <el-button-group>
                        <el-button type="success" icon="el-icon-plus" @click="add">
                            添加
                        </el-button>
                        <!-- <el-button type="primary" icon="el-icon-edit" @click="edit">编辑</el-button>
            <el-button type="danger" icon="el-icon-delete" @click="remove">删除</el-button>-->
                    </el-button-group>
                </el-form-item>
            </template>
            <!-- 默认搜索项 -->
            <el-form-item>
                <el-input v-model="filter.roleName" placeholder="角色名称" clearable />
            </el-form-item>
            <template slot="more">
                <!-- 更多搜索项 -->
            </template>
        </SimpleHead>
        <!-- table -->
        <SimpleTable ref="table" :url="tableUrl" :filter="filter" :offset="10">
            <el-table-column type="selection" width="40" fixed="left" />
            <el-table-column prop="roleId" label="角色ID" width="80" />
            <el-table-column prop="roleName" label="角色名称" width="220" />
            <el-table-column prop="remark" label="备注" />>
            <el-table-column prop="status" label="状态" width="120">
                <template slot-scope="scope">
                    <el-tag
                        :type="scope.row.isEnabled ? 'success' : 'danger'"
                        disable-transitions
                    >
                        {{ scope.row.isEnabled ? "启用" : "禁用" }}
                    </el-tag>
                </template>
            </el-table-column>
            <el-table-column prop="operate" label="操作" width="120">
                <template slot-scope="scope">
                    <el-button-group>
                        <el-button
                            type="success"
                            icon="el-icon-setting"
                            circle
                            size="mini"
                            title="分配菜单"
                            @click="setModuleRole(scope.row)"
                        />
                        <el-button
                            type="primary"
                            icon="el-icon-edit"
                            circle
                            size="mini"
                            title="编辑"
                            @click="edit(scope.row)"
                        />
                        <el-button
                            type="danger"
                            icon="el-icon-delete"
                            circle
                            size="mini"
                            title="删除"
                            @click="remove(scope.row)"
                        />
                    </el-button-group>
                </template>
            </el-table-column>
        </SimpleTable>
        <!-- 添加编辑 -->
        <AddEdit ref="edit" @on-ok="reloadTable" />
        <!-- 分配菜单权限 -->
        <SetModuleRole ref="setModuleRole" @on-ok="reloadTable" />
    </div>
</template>
<script>
export default {
    name: 'Role',
    components: {
        AddEdit: () => import('./addedit'), // 添加编辑
        SetModuleRole: () => import('./setmodulerole') // 分配菜单权限
    },
    data() {
        return {
            tableUrl: `${process.env.VUE_APP_API_ROOT}api/system/getrolelist`,
            // 过滤项
            filter: {
                userName: null,
                orgId: null
            }
        }
    },
    methods: {
        reloadTable(again) {
            this.$refs.table.reload(again)
        },
        onExpand() {
            this.$refs.table.resize()
        },
        treeNodeClick(data) {
            this.filter.orgId = data.id
            this.reloadTable()
        },
        add() {
            this.$refs.edit.open()
        },
        edit(row) {
            if (!row) {
                const rows = this.$refs.table.getSelection()
                if (rows.length !== 1) {
                    this.$message.warning('请选择单条数据进行操作.')
                    return
                }
                row = rows[0]
            }
            this.$refs.edit.open(row)
        },
        remove(row) {
            let ids = []
            if (!row) {
                const rows = this.$refs.table.getSelection()
                if (rows.length === 0) {
                    this.$message.warning('请选择要删除的数据')
                    return
                }
                ids = rows.map(e => e.userId)
            }

            this.$confirm('确定要删除角色吗？', '删除', {
                distinguishCancelAndClose: true,
                confirmButtonText: '确定',
                cancelButtonText: '取消'
            })
                .then(() => {
                    this.$api.post('api/system/roleremove', ids).then(res => {
                        if (res.succeeded) {
                            this.$message.success('删除成功.')
                            this.reloadTable()
                        } else {
                            this.$message.error(res.errors)
                        }
                    })
                })
                .catch(error => {
                    console.log(error)
                })
        },
        setModuleRole(row) {
            this.$refs.setModuleRole.open(row)
        }
    }
}
</script>
