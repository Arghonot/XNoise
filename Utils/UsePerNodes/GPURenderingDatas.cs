using UnityEngine;

namespace XNoise
{
    public class GPURenderingDatas
    {
        public Vector3 origin = Vector3.zero;
        public Vector3 scale = Vector3.one;
        public Vector3 rotation;
        public RenderTexture displacementMap;
        public float turbulencePower;
        public Vector4 quaternionRotation
        {
            get
            {
                Quaternion quat = Quaternion.Euler(rotation);
                return new Vector4(quat.x, quat.y, quat.z, quat.w);
            }
        }
        public RenderingAreaData area { get { return _area; } }
        public ProjectionType projection { get { return _projection; } }
        public Vector2 size { get { return _size; } }

        private RenderingAreaData _area;
        private ProjectionType _projection;
        private Vector2 _size;

        private void GetBlackTexture()
        {
            displacementMap = new RenderTexture((int)size.x, (int)size.y, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
            Graphics.Blit(Texture2D.blackTexture, displacementMap);
        }

        public GPURenderingDatas(Vector2 finalTextureSize, ProjectionType type, RenderingAreaData area)
        {
            this._area = area;
            this._projection = type;
            this._size = finalTextureSize;
            this.origin = Vector3.one;
            this.rotation = Vector3.zero;
            GetBlackTexture();
        }
    }
}