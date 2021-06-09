using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Joshua.Security.Security
{
    public class JoshuaPrincipal : IPrincipal
    {
        private IIdentity m_identity;
        private string[] m_roles;
        private string[] m_permissions;
        private int m_accessLevel;
        public IIdentity Identity => m_identity;
        public int AccessLevel => m_accessLevel;
        public JoshuaPrincipal(IIdentity identity, string[] roles, int accessLevel = 0, string[] permissions = null)
        {
            if (identity == null)
            {
                throw new ArgumentNullException("identity");
            }

            m_identity = identity;
            m_accessLevel = accessLevel;

            if (roles != null)
            {
                m_roles = new string[roles.Length];
                for (int i = 0; i < roles.Length; ++i)
                {
                    m_roles[i] = roles[i];
                }
            }
            else
            {
                m_roles = null;
            }

            if (permissions != null)
            {
                m_permissions = new string[permissions.Length];
                for (int i = 0; i < permissions.Length; ++i)
                {
                    m_permissions[i] = permissions[i];
                }
            }
            else
            {
                m_permissions = null;
            }
        }

        public bool IsInRole(string role)
        {
            if (IsDeveloper()) { return true; }

            if (role == null || m_roles == null)
            {
                return false;
            }

            return m_roles.Any(m_role => m_role != null && String.Compare(m_role, role, StringComparison.OrdinalIgnoreCase) == 0);
        }

        public bool HasPermission(string permission)
        {
            if (IsDeveloper()) { return true; }

            if (permission == null || m_permissions == null)
            {
                return false;
            }

            return m_permissions.Any(m_permission => m_permission != null && String.Compare(m_permission, permission, StringComparison.OrdinalIgnoreCase) == 0);
        }

        public bool IsInRoles(params string[] roles)
        {
            if (IsDeveloper()) { return true; }

            if (roles == null || m_roles == null)
            {
                return false;
            }

            return m_roles.Intersect(roles, StringComparer.OrdinalIgnoreCase).Any(m_role => m_role != null);
        }

        public bool HasPermissions(params string[] permissions)
        {
            if (IsDeveloper()) { return true; }

            if (permissions == null || m_permissions == null)
            {
                return false;
            }

            return m_permissions.Intersect(permissions, StringComparer.OrdinalIgnoreCase).Any(m_permission => m_permission != null);
        }

        public bool HasAccessLevel(int accessLevel)
        {
            if (IsDeveloper()) { return true; }

            return m_accessLevel >= accessLevel;
        }

        private bool IsDeveloper()
        {
            return m_roles.Any(role => role == "Developer");
        }
    }
}
