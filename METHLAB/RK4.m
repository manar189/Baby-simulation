function [t,z] = RK4(fun, u, z0, N, h)

t = (0:h:N*h-h);
z = zeros(length(z0), N);
z(:,1) = z0;    % initial condidtions

for k = 1:N-1 
    
    k1 = h*fun(u(:,k), z(:,k));
    k2 = h*fun(u(:,k), z(:,k)+k1/2);
    k3 = h*fun(u(:,k), z(:,k)+k2/2);
    k4 = h*fun(u(:,k), z(:,k)+k3);
    
    z(:,k+1) = z(:,k) + (1/6)*(k1 + 2*k2 + 2*k3 + k4); 
end

end