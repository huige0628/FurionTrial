<template>
    <div class="page-main">
        <el-row :gutter="6">
            <el-col :span="4">
                <OrgTree :is-root="false" @tree-node-click="treeNodeClick" />
            </el-col>
            <el-col :span="20">
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
                                <el-button
                                    type="success"
                                    icon="el-icon-plus"
                                    @click="add"
                                >
                                    添加
                                </el-button>
                                <!-- <el-button type="primary" icon="el-icon-edit" @click="edit(null)">编辑</el-button>
                <el-button type="danger" icon="el-icon-delete" @click="remove(null)">删除</el-button> -->
                            </el-button-group>
                        </el-form-item>
                    </template>
                    <!-- 默认搜索项 -->
                    <el-form-item>
                        <el-input
                            v-model="filter.userName"
                            placeholder="用户名"
                            clearable
                        />
                    </el-form-item>
                    <template slot="more">
                        <!-- 更多搜索项 -->
                    </template>
                </SimpleHead>
                <!-- table -->
                <SimpleTable
                    ref="table"
                    :url="tableUrl"
                    :filter="filter"
                    :offset="70"
                >
                    <el-table-column type="selection" width="40" fixed="left" />
                    <el-table-column
                        prop="userCode"
                        label="用户名"
                        width="100"
                    />
                    <el-table-column
                        prop="userName"
                        label="姓名"
                        width="100"
                    />
                    <el-table-column prop="sex" label="性别" width="80">
                        <template slot-scope="scope">
                            <el-tag
                                :type="scope.row.sex === 1 ? 'success' : ''"
                                effect="dark"
                                disable-transitions
                            >
                                {{ scope.row.sex === 1 ? "女" : "男" }}
                            </el-tag>
                        </template>
                    </el-table-column>
                    <el-table-column prop="avatar" label="头像" width="80" />>
                    <el-table-column prop="email" label="邮箱" />>
                    <el-table-column prop="phone" label="手机" />>
                    <el-table-column prop="qq" label="QQ" />>
                    <el-table-column prop="orgName" label="部门" />>
                    <el-table-column prop="status" label="状态" width="120">
                        <template slot-scope="scope">
                            <el-tag
                                :type="
                                    scope.row.status === 1
                                        ? 'success'
                                        : 'danger'
                                "
                                disable-transitions
                            >
                                {{ scope.row.status === 1 ? "启用" : "禁用" }}
                            </el-tag>
                        </template>
                    </el-table-column>
                    <el-table-column prop="operate" label="操作" width="120">
                        <template slot-scope="scope">
                            <el-button-group>
                                <el-button
                                    type="success"
                                    icon="el-icon-user"
                                    circle
                                    size="mini"
                                    title="设置角色"
                                    @click="setRole(scope.row)"
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
            </el-col>
        </el-row>
        <AddEdit ref="edit" @on-ok="reloadTable" />
        <SetRole ref="setRole" @on-ok="reloadTable" />
    </div>
</template>
<script>
const url = `${process.env.VUE_APP_API_ROOT}api/system/getuserlist`
export default {
    name: 'UserManager',
    components: {
        AddEdit: () => import('./addedit'), // 添加编辑
        SetRole: () => import('./setrole'), // 添加角色
        OrgTree: () => import('../org/orgtree') // 部门树组件
    },
    data() {
        return {
            tableUrl: url,
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
            this.$refs.edit.open(row)
        },
        remove(row) {
            let ids = []
            if (!row) {
                const rows = this.$refs.table.getSelection()
                if (rows.length === 0) {
                    this.$message.warning('请选择要删除的栏目')
                    return
                }
                ids = rows.map(e => e.userId)
            }

            this.$confirm('确定要删除用户信息吗？', '删除', {
                distinguishCancelAndClose: true,
                confirmButtonText: '确定',
                cancelButtonText: '取消'
            })
                .then(() => {
                    this.$api.post('api/system/userremove', ids).then(res => {
                        if (res.succeeded) {
                            this.$message.success(res.errors)
                            this.reloadTable()
                        } else {
                            this.$message.error(res.errors)
                        }
                    })
                })
                .catch(error => {
                    this.$message.error(error)
                })
        },
        setRole(row) {
            this.$refs.setRole.open(row)
        }
    }
}
</script>
