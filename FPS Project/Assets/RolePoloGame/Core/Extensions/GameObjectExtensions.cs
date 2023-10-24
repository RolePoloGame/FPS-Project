using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RolePoloGame.Core.Extensions
{
    public static class GameObjectExtensions
    {
        public static T CollectComponent<T>(this GameObject gameObject) where T : Component
        {
            var component = gameObject.GetComponent<T>();
            if (component == null)
            {
                component = gameObject.AddComponent<T>();
            }

            return component;
        }

        public static Bounds GetColliderBounds(this GameObject gameObject)
        {
            Collider[] colliders = gameObject.GetComponentsInChildren<Collider>();
            if (colliders.Length == 0)
            {
                return new Bounds(gameObject.transform.position, Vector3.one);
            }

            Bounds bounds = colliders[0].bounds;
            for (var i = 1; i < colliders.Length; i++)
            {
                var coll = colliders[i];
                bounds.Encapsulate(coll.bounds);
            }

            return bounds;
        }

        public static T GetComponentByRay<T>(Ray ray, LayerMask layerMask) where T : Component
        {
            if (!Physics.Raycast(ray, out var hit, float.PositiveInfinity, layerMask))
                return null;
            var componentInParent = hit.transform.GetComponentInParent<T>();
            if (componentInParent != null) return componentInParent;
            var componentInChildren = hit.transform.GetComponentInChildren<T>();
            return componentInChildren;
        }

        public static T GetNearestOfType<T>(T source, List<T> targets) where T : Component
        {
            T currentTarget = null;
            var oldDistance = 0.0f;
            foreach (var target in targets)
            {
                if (currentTarget == null)
                {
                    currentTarget = target;
                    oldDistance = Vector3.Distance(source.transform.position, currentTarget.transform.position);
                    continue;
                }

                var newDistance = Vector3.Distance(source.transform.position, target.transform.position);
                if (!(newDistance < oldDistance)) continue;
                currentTarget = target;
                oldDistance = newDistance;
            }

            return currentTarget;
        }

        public static List<T> FindComponentsAtPosition<T>(Vector3 position, float scanSize = .5f, int layer = 0,
            int maxCollisions = 30) where T : Component
        {
            List<T> found = new();
            var collidersBuffer = new Collider[maxCollisions];
            var size = Physics.OverlapSphereNonAlloc(position, scanSize, collidersBuffer);
            if (size == 0) return found;

            maxCollisions = size < maxCollisions ? size : maxCollisions;
            for (var i = 0; i < maxCollisions; i++)
            {
                var candidateTransform = collidersBuffer[i].transform;

                if (candidateTransform == null) continue;
                if (candidateTransform.TryGetComponent<T>(out var collided))
                {
                    if (found.Contains(collided)) continue;
                    found.Add(collided);
                    continue;
                }

                collided = candidateTransform.GetComponentInParent<T>();

                if (collided == null) continue;
                if (found.Contains(collided)) continue;
                found.Add(collided);
            }

            return found;
        }

        public static T FindComponentAtPosition<T>(Vector3 position, float scanSize = .5f, int layer = 0,
            int maxCollisions = 30) where T : Component
        {
            var collidersBuffer = new Collider[maxCollisions];
            var size = Physics.OverlapSphereNonAlloc(position, scanSize, collidersBuffer);
            if (size == 0) return null;

            maxCollisions = size < maxCollisions ? size : maxCollisions;
            for (var i = 0; i < maxCollisions; i++)
            {
                var candidateTransform = collidersBuffer[i].transform;

                if (candidateTransform == null) continue;
                if (candidateTransform.TryGetComponent<T>(out var collided))
                    return collided;

                collided = candidateTransform.GetComponentInParent<T>();

                if (collided == null) continue;
                return collided;
            }

            return null;
        }

        public static IEnumerator RotateInTime(Transform target, float endRotation, float maxTime)
        {
            var startRotation = target.eulerAngles.y;
            var timer = 0.0f;
            var loopCondition = true;

            while (loopCondition)
            {
                loopCondition = timer < maxTime;
                timer += Time.deltaTime;
                var yRotation = Mathf.LerpAngle(startRotation, endRotation, timer / maxTime);
                target.eulerAngles = target.eulerAngles.WithY(yRotation);
                yield return null;
                if (Mathf.Abs(endRotation - yRotation) <= float.Epsilon)
                    loopCondition = false;
            }

            target.eulerAngles = target.eulerAngles.WithY(endRotation);
        }

        public static void Rotate(this Transform target, float endRotation, bool additive = false)
        {
            var eulerAngles = target.localEulerAngles;
            target.localEulerAngles = additive
                ? eulerAngles.WithY(eulerAngles.y + endRotation)
                : eulerAngles.WithY(endRotation);
        }

        /// <summary>
        /// by VesuvianPrime
        /// https://answers.unity.com/questions/894959/addingremoving-objects-in-editor-mode.html
        /// </summary>
        public static T SafeDestroy<T>(T obj, float time = 0.0f) where T : Object
        {
            if (Application.isEditor)
                Object.DestroyImmediate(obj);
            else
                Object.Destroy(obj, time);

            return null;
        }

        /// <summary>
        /// by VesuvianPrime
        /// https://answers.unity.com/questions/894959/addingremoving-objects-in-editor-mode.html
        /// </summary>
        public static T SafeDestroyGameObject<T>(T component, float time = 0.0f) where T : Component
        {
            if (component != null)
                SafeDestroy(component.gameObject, time);
            return null;
        }

        public static GameObject SpawnTempBall(Vector3 worldPosition, Vector3 worldScale, Color color,
            float time = 5.0f)
        {
            var go = CreateSphere(worldPosition, worldScale, color);
            if (time > 0.0f)
                Object.Destroy(go, time);
            return go;
        }

        public static GameObject CreateSphere(Vector3 worldPosition, Vector3 worldScale, Color color)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            go.transform.position = worldPosition;
            go.transform.localScale = worldScale;
            go.GetComponent<MeshRenderer>().material.color = color;
            Object.Destroy(go.GetComponent<Collider>());
            return go;
        }

        public static GameObject SpawnTempBall(Vector3 worldPosition, Vector3 worldScale, System.Drawing.Color color,
            float time = 5.0f)
        {
            return SpawnTempBall(worldPosition, worldScale, color.ToUnityColor(), time);
        }

        public static GameObject SpawnTempBall(Vector3 worldPosition, Color color, float time)
        {
            return SpawnTempBall(worldPosition, Vector3.one,
                color, time);
        }

        public static GameObject SpawnTempBall(Vector3 worldPosition, float time)
        {
            return SpawnTempBall(worldPosition, Vector3.one, Color.white,
                time);
        }

        public static GameObject SpawnTempBall(Vector3 worldPosition, Color color)
        {
            return SpawnTempBall(worldPosition, Vector3.one, color, 0.0f);
        }

        public static GameObject SpawnTempBall(Transform transform, Vector3 worldPosition)
        {
            return SpawnTempBall(worldPosition, Vector3.one, Color.white,
                0.0f);
        }

        public static LineRenderer SpawnLineRenderer(List<Vector3> positions, float time = 8.0f)
        {
            var go = new GameObject("Line_Render");
            go.transform.position = Vector3.zero;
            var lineRenderer = go.AddComponent<LineRenderer>();
            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            lineRenderer.widthMultiplier = 0.2f;
            lineRenderer.startWidth = .25f;
            lineRenderer.endWidth = .25f;
            lineRenderer.startColor = Color.white;
            lineRenderer.endColor = Color.white;
            if (time > 0.0f)
                SafeDestroyGameObject(lineRenderer, time);
            var points = positions.ToArray();
            Debug.Log($"Line with {points.Length} points");
            lineRenderer.positionCount = points.Length;
            lineRenderer.SetPositions(points);
            return lineRenderer;
        }
    }
}